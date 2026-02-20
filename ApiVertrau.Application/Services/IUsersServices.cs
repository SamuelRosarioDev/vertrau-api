using ApiVertrau.Application.DTOs;

namespace ApiVertrau.Application.Services;

public interface IUsersServices
{
    Task<UsuarioResponseDTO> CreateAsync(CreateUsuarioDTO dto);
    Task<UsuarioResponseDTO?> GetByIdAsync(long id);
    Task<IEnumerable<UsuarioResponseDTO>> GetAllAsync();
    Task<UsuarioResponseDTO?> GetByEmailAsync(string email);
    Task UpdateAsync(long id, UsuarioUpdateDTO dto);
    Task PatchAsync(long id, UsuarioPatchDTO dto);
    Task DeleteAsync(long id);
}
