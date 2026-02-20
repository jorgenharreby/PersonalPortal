using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
}
