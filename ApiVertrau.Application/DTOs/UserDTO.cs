using System.ComponentModel.DataAnnotations;
using ApiVertrau.Domain.Enums;

namespace ApiVertrau.Application.DTOs;

public class CreateUsuarioDTO
{
    /// <summary>Primeiro nome do usuário.</summary>
    /// <example>Erick</example>
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MinLength(2, ErrorMessage = "Nome deve ter pelo menos 2 caracteres.")]
    public required string Nome { get; set; }

    /// <summary>Sobrenome completo.</summary>
    /// <example>Silva</example>
    [Required(ErrorMessage = "Sobrenome é obrigatório.")]
    [MinLength(2, ErrorMessage = "Sobrenome deve ter pelo menos 2 caracteres.")]
    public required string Sobrenome { get; set; }

    /// <summary>E-mail institucional ou pessoal.</summary>
    /// <example>erick.silva@vertrau.com.br</example>
    [Required(ErrorMessage = "E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public required string Email { get; set; }

    /// <summary>Gênero do usuário (0 = Masculino, 1 = Feminino, 2 = Outro).</summary>
    /// <example>0</example>
    [Required(ErrorMessage = "Gênero é obrigatório.")]
    public required Gender Genero { get; set; }

    /// <summary>Data de nascimento no formato YYYY-MM-DD.</summary>
    /// <example>1995-05-15</example>
    public DateOnly? DataNascimento { get; set; }
}

public class UsuarioResponseDTO
{
    /// <example>1</example>
    public long Id { get; set; }

    /// <example>Erick</example>
    public string Nome { get; set; } = string.Empty;

    /// <example>Silva</example>
    public string Sobrenome { get; set; } = string.Empty;

    /// <example>erick.silva@vertrau.com.br</example>
    public string Email { get; set; } = string.Empty;

    /// <example>MASCULINO</example>
    public string Genero { get; set; } = string.Empty;

    /// <example>1995-05-15</example>
    public DateOnly? DataNascimento { get; set; }
}

public class UsuarioUpdateDTO
{
    /// <example>Erick</example>
    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MinLength(2, ErrorMessage = "Nome deve ter pelo menos 2 caracteres.")]
    public required string Nome { get; set; } = string.Empty;

    /// <example>Silva</example>
    [Required(ErrorMessage = "Sobrenome é obrigatório.")]
    [MinLength(2, ErrorMessage = "Sobrenome deve ter pelo menos 2 caracteres.")]
    public required string Sobrenome { get; set; } = string.Empty;

    /// <example>erick.silva@vertrau.com.br</example>
    [Required(ErrorMessage = "E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public required string Email { get; set; } = string.Empty;

    /// <example>0</example>
    [Required(ErrorMessage = "Gênero é obrigatório.")]
    public required Gender Genero { get; set; }

    /// <example>1995-05-15</example>
    public DateOnly? DataNascimento { get; set; }
}

public class UsuarioPatchDTO
{
    /// <example>Erick Atualizado</example>
    [MinLength(2, ErrorMessage = "Nome deve ter pelo menos 2 caracteres.")]
    public string? Nome { get; set; }

    /// <example>Silva</example>
    [MinLength(2, ErrorMessage = "Sobrenome deve ter pelo menos 2 caracteres.")]
    public string? Sobrenome { get; set; }

    /// <example>novo.email@vertrau.com.br</example>
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string? Email { get; set; }

    public Gender? Genero { get; set; }

    public DateOnly? DataNascimento { get; set; }
}
