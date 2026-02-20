using System.Net.Mime;
using ApiVertrau.Application.DTOs;
using ApiVertrau.Application.Interfaces;
using ApiVertrau.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiVertrau.API.Controllers;

/// <summary>
/// Gerenciamento de usuários do sistema.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class UsersController : ControllerBase
{
    private readonly IUsersServices _service;

    public UsersController(IUsersServices service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtém a lista completa de usuários cadastrados.
    /// </summary>
    /// <response code="200">Retorna a lista de usuários com sucesso.</response>
    /// <response code="500">Erro interno no servidor.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await _service.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Busca um usuário pelo seu identificador único.
    /// </summary>
    /// <param name="id">ID do usuário (long).</param>
    /// <response code="200">Usuário encontrado com sucesso.</response>
    /// <response code="404">Usuário não encontrado para o ID fornecido.</response>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { message = $"Usuário com ID {id} não encontrado." });
        return Ok(user);
    }

    /// <summary>
    /// Realiza o cadastro de um novo usuário.
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /api/v1/users
    ///     {
    ///        "nome": "João",
    ///        "sobrenome": "Silva",
    ///        "email": "joao@vertrau.com.br",
    ///        "genero": 0,
    ///        "dataNascimento": "1990-01-15"
    ///     }
    ///
    /// Valores para o campo genero:
    /// - 0: Masculino
    /// - 1: Feminino
    /// - 2: Outro
    /// </remarks>
    /// <param name="dto">Dados para criação do usuário.</param>
    /// <response code="201">Usuário criado com sucesso.</response>
    /// <response code="400">Dados da requisição inválidos.</response>
    /// <response code="409">Já existe um usuário cadastrado com este e-mail.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUsuarioDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _service.CreateAsync(dto);
        return Created(string.Empty, user);
    }

    /// <summary>
    /// Atualiza todos os dados de um usuário (substituição completa).
    /// </summary>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     PUT /api/v1/users/1
    ///     {
    ///        "nome": "João",
    ///        "sobrenome": "Silva",
    ///        "email": "joao@vertrau.com.br",
    ///        "genero": 0,
    ///        "dataNascimento": "1990-01-15"
    ///     }
    /// </remarks>
    /// <param name="id">ID do usuário a ser atualizado.</param>
    /// <param name="dto">Dados completos para substituição do usuário.</param>
    /// <response code="204">Usuário atualizado com sucesso.</response>
    /// <response code="400">Dados da requisição inválidos.</response>
    /// <response code="404">Usuário não encontrado.</response>
    /// <response code="409">Já existe um usuário cadastrado com este e-mail.</response>
    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(long id, [FromBody] UsuarioUpdateDTO dto)
    {
        var userExists = await _service.GetByIdAsync(id);
        if (userExists == null)
            return NotFound();

        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Atualiza campos parciais de um usuário.
    /// </summary>
    /// <remarks>
    /// Apenas os campos informados serão atualizados. Campos nulos são ignorados.
    ///
    ///     PATCH /api/v1/users/1
    ///     {
    ///        "email": "novoemail@vertrau.com.br"
    ///     }
    /// </remarks>
    /// <param name="id">ID do usuário a ser atualizado parcialmente.</param>
    /// <param name="dto">Campos a serem atualizados.</param>
    /// <response code="204">Atualização parcial realizada com sucesso.</response>
    /// <response code="404">Usuário não encontrado.</response>
    /// <response code="409">Já existe um usuário cadastrado com este e-mail.</response>
    [HttpPatch("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PatchAsync(long id, [FromBody] UsuarioPatchDTO dto)
    {
        await _service.PatchAsync(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Exclui permanentemente um usuário do sistema.
    /// </summary>
    /// <param name="id">ID do usuário a ser excluído.</param>
    /// <response code="204">Usuário removido com sucesso.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
