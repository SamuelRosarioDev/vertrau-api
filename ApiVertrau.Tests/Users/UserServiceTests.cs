using ApiVertrau.Exceptions;
using ApiVertrau.Modules.Users.Application.DTOs;
using ApiVertrau.Modules.Users.Application.Services;
using ApiVertrau.Modules.Users.Domain;
using ApiVertrau.Modules.Users.Domain.Enums;
using ApiVertrau.Modules.Users.Infrastructure.Repositories;
using Moq;

namespace ApiVertrau.Tests.Users;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repositoryMock;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IUserRepository>();
        _service = new UserService(_repositoryMock.Object);
    }

    // ─── CREATE ───────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_DeveRetornarUsuario_QuandoDadosValidos()
    {
        // Arrange
        var dto = new CreateUsuarioDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            Email = "joao@vertrau.com.br",
            Genero = Gender.Masculino,
            DataNascimento = new DateOnly(1990, 1, 15),
        };

        _repositoryMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync((User?)null);
        _repositoryMock.Setup(r => r.Create(It.IsAny<User>())).ReturnsAsync(1L);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Nome, result.Nome);
        Assert.Equal(dto.Email, result.Email);
    }

    [Fact]
    public async Task CreateAsync_DeveLancarConflictException_QuandoEmailJaExiste()
    {
        // Arrange
        var dto = new CreateUsuarioDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            Email = "joao@vertrau.com.br",
            Genero = Gender.Masculino,
        };

        var usuarioExistente = CriarUsuario();
        _repositoryMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync(usuarioExistente);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateAsync(dto));
    }

    // ─── GET BY ID ────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_DeveRetornarUsuario_QuandoExiste()
    {
        // Arrange
        var usuario = CriarUsuario();
        _repositoryMock.Setup(r => r.GetById(1L)).ReturnsAsync(usuario);

        // Act
        var result = await _service.GetByIdAsync(1L);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(usuario.Nome, result.Nome);
        Assert.Equal(usuario.Email, result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_DeveLancarNotFoundException_QuandoNaoExiste()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99L)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(99L));
    }

    // ─── GET ALL ──────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeUsuarios()
    {
        // Arrange
        var usuarios = new List<User> { CriarUsuario(), CriarUsuario() };
        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(usuarios);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaVazia_QuandoNaoHaUsuarios()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<User>());

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    // ─── UPDATE ───────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_DeveAtualizar_QuandoDadosValidos()
    {
        // Arrange
        var usuario = CriarUsuario();
        var dto = new UsuarioUpdateDTO
        {
            Nome = "João Atualizado",
            Sobrenome = "Silva",
            Email = "novo@vertrau.com.br",
            Genero = Gender.Masculino,
        };

        _repositoryMock.Setup(r => r.GetById(1L)).ReturnsAsync(usuario);
        _repositoryMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync((User?)null);
        _repositoryMock.Setup(r => r.Update(It.IsAny<User>())).Returns(Task.CompletedTask);

        // Act & Assert (não deve lançar exceção)
        await _service.UpdateAsync(1L, dto);
        _repositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarNotFoundException_QuandoUsuarioNaoExiste()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99L)).ReturnsAsync((User?)null);

        var dto = new UsuarioUpdateDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            Email = "joao@vertrau.com.br",
            Genero = Gender.Masculino,
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(99L, dto));
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarConflictException_QuandoEmailPertenceAOutroUsuario()
    {
        // Arrange
        var usuario = CriarUsuario();
        var outroUsuario = new User(
            "Outro",
            "User",
            "outro@vertrau.com.br",
            Gender.Masculino,
            null
        );

        var dto = new UsuarioUpdateDTO
        {
            Nome = "João",
            Sobrenome = "Silva",
            Email = outroUsuario.Email,
            Genero = Gender.Masculino,
        };

        _repositoryMock.Setup(r => r.GetById(1L)).ReturnsAsync(usuario);
        _repositoryMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync(outroUsuario);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _service.UpdateAsync(1L, dto));
    }

    // ─── PATCH ────────────────────────────────────────────────

    [Fact]
    public async Task PatchAsync_DeveAtualizarParcialmente_QuandoDadosValidos()
    {
        // Arrange
        var usuario = CriarUsuario();
        var dto = new UsuarioPatchDTO { Nome = "Novo Nome" };

        _repositoryMock.Setup(r => r.GetById(1L)).ReturnsAsync(usuario);
        _repositoryMock.Setup(r => r.Update(It.IsAny<User>())).Returns(Task.CompletedTask);

        // Act
        await _service.PatchAsync(1L, dto);

        // Assert
        _repositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task PatchAsync_DeveLancarNotFoundException_QuandoUsuarioNaoExiste()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99L)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.PatchAsync(99L, new UsuarioPatchDTO())
        );
    }

    // ─── DELETE ───────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_DeveDeletar_QuandoUsuarioExiste()
    {
        // Arrange
        var usuario = CriarUsuario();
        _repositoryMock.Setup(r => r.GetById(1L)).ReturnsAsync(usuario);
        _repositoryMock.Setup(r => r.Delete(1L)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1L);

        // Assert
        _repositoryMock.Verify(r => r.Delete(1L), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveLancarNotFoundException_QuandoUsuarioNaoExiste()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetById(99L)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync(99L));
    }

    // ─── HELPER ───────────────────────────────────────────────

    private static User CriarUsuario(string email = "joao@vertrau.com.br")
    {
        return new User("João", "Silva", email, Gender.Masculino, new DateOnly(1990, 1, 15));
    }
}
