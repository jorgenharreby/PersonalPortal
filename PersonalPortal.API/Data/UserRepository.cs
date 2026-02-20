using Dapper;
using Microsoft.Data.SqlClient;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT Id, Username, Password, Role, DisplayName FROM Users WHERE Username = @Username",
            new { Username = username });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT Id, Username, Password, Role, DisplayName FROM Users WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<User>(
            "SELECT Id, Username, Password, Role, DisplayName FROM Users");
    }
}
