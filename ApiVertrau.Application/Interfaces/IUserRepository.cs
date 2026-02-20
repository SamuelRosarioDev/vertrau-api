using ApiVertrau.Domain.Entities;

namespace ApiVertrau.Application.Interfaces;

public interface IUserRepository
{
    Task<long> Create(User user);
    Task<User?> GetById(long id);
    Task<User?> GetByEmail(string email);
    Task<IEnumerable<User>> GetAll();
    Task Update(User user);
    Task Delete(long id);
    Task<bool> Exists(long id);
}
