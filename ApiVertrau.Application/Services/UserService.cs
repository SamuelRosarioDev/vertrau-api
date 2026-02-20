using ApiVertrau.Application.DTOs;
using ApiVertrau.Application.Interfaces;
using ApiVertrau.Application.Mappers;
using ApiVertrau.Domain.Entities;
using ApiVertrau.Domain.Enums;
using ApiVertrau.Domain.Exceptions;

namespace ApiVertrau.Application.Services;

public class UserService : IUsersServices
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<UsuarioResponseDTO> CreateAsync(CreateUsuarioDTO dto)
    {
        var emailExistente = await _repository.GetByEmail(dto.Email);
        if (emailExistente is not null)
            throw new ConflictException("Já existe um usuário cadastrado com este e-mail.");

        var user = UserMapper.ToDomain(dto);
        var id = await _repository.Create(user);

        return new UsuarioResponseDTO
        {
            Id = id,
            Nome = dto.Nome,
            Sobrenome = dto.Sobrenome,
            Email = dto.Email,
            Genero =
                dto.Genero == Gender.Masculino ? "MASCULINO"
                : dto.Genero == Gender.Feminino ? "FEMININO"
                : "OUTRO",
            DataNascimento = dto.DataNascimento,
        };
    }

    public async Task<UsuarioResponseDTO?> GetByIdAsync(long id)
    {
        var user = await _repository.GetById(id);
        if (user is null)
            throw new NotFoundException("Usuário não encontrado.");
        return UserMapper.ToResponse(user);
    }

    public async Task<IEnumerable<UsuarioResponseDTO>> GetAllAsync()
    {
        var users = await _repository.GetAll();
        return users.Select(UserMapper.ToResponse);
    }

    public async Task<UsuarioResponseDTO?> GetByEmailAsync(string email)
    {
        var user = await _repository.GetByEmail(email);
        if (user is null)
            throw new NotFoundException("Usuário não encontrado.");
        return UserMapper.ToResponse(user);
    }

    public async Task UpdateAsync(long id, UsuarioUpdateDTO dto)
    {
        var user = await _repository.GetById(id);
        if (user is null)
            throw new NotFoundException("Usuário não encontrado.");

        var emailExistente = await _repository.GetByEmail(dto.Email);
        if (emailExistente is not null && emailExistente.Id != id)
            throw new ConflictException("Já existe um usuário cadastrado com este e-mail.");

        user.Update(dto.Nome, dto.Sobrenome, dto.Email, dto.Genero, dto.DataNascimento);
        await _repository.Update(user);
    }

    public async Task PatchAsync(long id, UsuarioPatchDTO dto)
    {
        var user = await _repository.GetById(id);
        if (user is null)
            throw new NotFoundException("Usuário não encontrado.");

        if (dto.Email is not null)
        {
            var emailExistente = await _repository.GetByEmail(dto.Email);
            if (emailExistente is not null && emailExistente.Id != id)
                throw new ConflictException("Já existe um usuário cadastrado com este e-mail.");
        }

        user.Patch(dto.Nome, dto.Sobrenome, dto.Email, dto.Genero, dto.DataNascimento);
        await _repository.Update(user);
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _repository.GetById(id);
        if (user is null)
            throw new NotFoundException("Usuário não encontrado.");
        await _repository.Delete(id);
    }
}
