using ApiVertrau.Application.DTOs;
using ApiVertrau.Domain.Entities;
using ApiVertrau.Domain.Enums;

namespace ApiVertrau.Application.Mappers;

public static class UserMapper
{
    public static UsuarioResponseDTO ToResponse(User user) =>
        new()
        {
            Id = user.Id,
            Nome = user.Nome,
            Sobrenome = user.Sobrenome,
            Email = user.Email,
            Genero =
                user.Genero == Gender.Feminino ? "FEMININO"
                : user.Genero == Gender.Masculino ? "MASCULINO"
                : "OUTRO",
            DataNascimento = user.DataNascimento,
        };

    public static User ToDomain(CreateUsuarioDTO dto) =>
        new(dto.Nome, dto.Sobrenome, dto.Email, dto.Genero, dto.DataNascimento);
}
