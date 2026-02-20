using System.Data;
using ApiVertrau.Application.Interfaces;
using ApiVertrau.Domain.Entities;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace ApiVertrau.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    private IDbConnection CreateConnection() => new SqliteConnection(_connectionString);

    public async Task<long> Create(User user)
    {
        using var connection = CreateConnection();
        var sql =
            @"
            INSERT INTO user (nome, sobrenome, email, genero, datanascimento, createdat)
            VALUES (@Nome, @Sobrenome, @Email, @Genero, @DataNascimento, @CreatedAt);
            SELECT last_insert_rowid();";
        return await connection.ExecuteScalarAsync<long>(sql, user);
    }

    public async Task<User?> GetById(long id)
    {
        using var connection = CreateConnection();
        var sql =
            "SELECT id, nome, sobrenome, email, genero, datanascimento, createdat FROM user WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByEmail(string email)
    {
        using var connection = CreateConnection();
        var sql =
            "SELECT id, nome, sobrenome, email, genero, datanascimento, createdat FROM user WHERE email = @Email";
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        using var connection = CreateConnection();
        var sql = "SELECT id, nome, sobrenome, email, genero, datanascimento, createdat FROM user";
        return await connection.QueryAsync<User>(sql);
    }

    public async Task Update(User user)
    {
        using var connection = CreateConnection();
        var sql =
            @"
            UPDATE user
            SET nome = @Nome,
                sobrenome = @Sobrenome,
                email = @Email,
                genero = @Genero,
                datanascimento = @DataNascimento
            WHERE id = @Id;";
        await connection.ExecuteAsync(sql, user);
    }

    public async Task Delete(long id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM user WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<bool> Exists(long id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT 1 FROM user WHERE id = @Id";
        var count = await connection.ExecuteScalarAsync<int>(sql, new { Id = id });
        return count > 0;
    }
}
