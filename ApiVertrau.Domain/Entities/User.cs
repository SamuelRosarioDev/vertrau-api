using ApiVertrau.Domain.Enums;

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
        Nome = nome;
        Sobrenome = sobrenome;
        Email = email;
        Genero = genero;
        DataNascimento = dataNascimento;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string nome,
        string sobrenome,
        string email,
        Gender genero,
        DateOnly? dataNascimento
    )
    {
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
            Nome = nome;
        if (sobrenome is not null)
            Sobrenome = sobrenome;
        if (email is not null)
            Email = email;
        if (genero.HasValue)
            Genero = genero.Value;
        if (dataNascimento.HasValue)
            DataNascimento = dataNascimento;
    }
}
