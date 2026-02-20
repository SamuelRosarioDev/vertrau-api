using ApiVertrau.Domain.Enums;
using ApiVertrau.Domain.Exceptions;

namespace ApiVertrau.Domain.Entities;

public class User
{
    public long Id { get; private set; }
    public string Nome { get; private set; }
    public string Sobrenome { get; private set; }
    public string Email { get; private set; }
    public Gender Genero { get; private set; }
    public DateOnly? DataNascimento { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected User()
    {
        Nome = null!;
        Sobrenome = null!;
        Email = null!;
    }

    public User(
        string nome,
        string sobrenome,
        string email,
        Gender genero,
        DateOnly? dataNascimento
    )
    {
        Validate(nome, sobrenome, email, genero, dataNascimento);
        Nome = nome;
        Sobrenome = sobrenome;
        Email = email;
        Genero = genero;
        DataNascimento = dataNascimento;
        CreatedAt = DateTime.UtcNow;
    }

    private void Validate(
        string nome,
        string sobrenome,
        string email,
        Gender genero,
        DateOnly? dataNascimento
    )
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(sobrenome))
            throw new DomainException("Sobrenome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email é obrigatório.");
        if (!Enum.IsDefined(typeof(Gender), genero))
            throw new DomainException("Gênero inválido.");
        if (dataNascimento.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (dataNascimento > today)
                throw new DomainException("Data de nascimento não pode ser futura.");
        }
    }

    public void Update(
        string nome,
        string sobrenome,
        string email,
        Gender genero,
        DateOnly? dataNascimento
    )
    {
        Validate(nome, sobrenome, email, genero, dataNascimento);
        Nome = nome;
        Sobrenome = sobrenome;
        Email = email;
        Genero = genero;
        DataNascimento = dataNascimento;
    }

    public void Patch(
        string? nome,
        string? sobrenome,
        string? email,
        Gender? genero,
        DateOnly? dataNascimento
    )
    {
        if (nome is not null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome não pode ser vazio.");
            Nome = nome;
        }
        if (sobrenome is not null)
        {
            if (string.IsNullOrWhiteSpace(sobrenome))
                throw new DomainException("Sobrenome não pode ser vazio.");
            Sobrenome = sobrenome;
        }
        if (email is not null)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email não pode ser vazio.");
            Email = email;
        }
        if (genero.HasValue)
        {
            if (!Enum.IsDefined(typeof(Gender), genero.Value))
                throw new DomainException("Gênero inválido.");
            Genero = genero.Value;
        }
        if (dataNascimento.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (dataNascimento > today)
                throw new DomainException("Data de nascimento não pode ser futura.");
            DataNascimento = dataNascimento;
        }
    }
}
