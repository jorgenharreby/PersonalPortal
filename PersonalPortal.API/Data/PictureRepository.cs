using Dapper;
using Microsoft.Data.SqlClient;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public class PictureRepository : IPictureRepository
{
    private readonly string _connectionString;

    public PictureRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<Picture?> GetByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Picture>(
            "SELECT Id, FileName, ImageData, Caption, RecipeId, Created, Updated FROM Pictures WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<IEnumerable<Picture>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Picture>(
            "SELECT Id, FileName, ImageData, Caption, RecipeId, Created, Updated FROM Pictures ORDER BY Updated DESC");
    }

    public async Task<IEnumerable<Picture>> GetLatestAsync(int count)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Picture>(
            "SELECT TOP (@Count) Id, FileName, ImageData, Caption, RecipeId, Created, Updated FROM Pictures ORDER BY Updated DESC",
            new { Count = count });
    }

    public async Task<IEnumerable<Picture>> GetByRecipeIdAsync(Guid recipeId)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Picture>(
            "SELECT Id, FileName, ImageData, Caption, RecipeId, Created, Updated FROM Pictures WHERE RecipeId = @RecipeId ORDER BY Created",
            new { RecipeId = recipeId });
    }

    public async Task<Guid> CreateAsync(Picture picture)
    {
        using var connection = new SqlConnection(_connectionString);
        picture.Id = Guid.NewGuid();
        picture.Created = DateTime.UtcNow;
        picture.Updated = DateTime.UtcNow;
        
        await connection.ExecuteAsync(
            "INSERT INTO Pictures (Id, FileName, ImageData, Caption, RecipeId, Created, Updated) VALUES (@Id, @FileName, @ImageData, @Caption, @RecipeId, @Created, @Updated)",
            picture);
        
        return picture.Id;
    }

    public async Task<bool> UpdateAsync(Picture picture)
    {
        using var connection = new SqlConnection(_connectionString);
        picture.Updated = DateTime.UtcNow;
        
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE Pictures SET FileName = @FileName, ImageData = @ImageData, Caption = @Caption, RecipeId = @RecipeId, Updated = @Updated WHERE Id = @Id",
            picture);
        
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM Pictures WHERE Id = @Id",
            new { Id = id });
        
        return rowsAffected > 0;
    }
}
