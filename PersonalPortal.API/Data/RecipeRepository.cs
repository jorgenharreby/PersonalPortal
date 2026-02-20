using Dapper;
using Microsoft.Data.SqlClient;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public class RecipeRepository : IRecipeRepository
{
    private readonly string _connectionString;
    private readonly IPictureRepository _pictureRepository;

    public RecipeRepository(IConfiguration configuration, IPictureRepository pictureRepository)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException(nameof(configuration));
        _pictureRepository = pictureRepository;
    }

    public async Task<Recipe?> GetByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        var recipe = await connection.QueryFirstOrDefaultAsync<Recipe>(
            "SELECT Id, Name, Type, RecipeText, Created, Updated FROM Recipes WHERE Id = @Id",
            new { Id = id });
        
        if (recipe != null)
        {
            recipe.Pictures = (await _pictureRepository.GetByRecipeIdAsync(id)).ToList();
        }
        
        return recipe;
    }

    public async Task<IEnumerable<Recipe>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        var recipes = (await connection.QueryAsync<Recipe>(
            "SELECT Id, Name, Type, RecipeText, Created, Updated FROM Recipes ORDER BY Updated DESC")).ToList();
        
        foreach (var recipe in recipes)
        {
            recipe.Pictures = (await _pictureRepository.GetByRecipeIdAsync(recipe.Id)).ToList();
        }
        
        return recipes;
    }

    public async Task<IEnumerable<Recipe>> GetLatestAsync(int count)
    {
        using var connection = new SqlConnection(_connectionString);
        var recipes = (await connection.QueryAsync<Recipe>(
            "SELECT TOP (@Count) Id, Name, Type, RecipeText, Created, Updated FROM Recipes ORDER BY Updated DESC",
            new { Count = count })).ToList();
        
        foreach (var recipe in recipes)
        {
            recipe.Pictures = (await _pictureRepository.GetByRecipeIdAsync(recipe.Id)).ToList();
        }
        
        return recipes;
    }

    public async Task<IEnumerable<Recipe>> GetByTypeAsync(string type)
    {
        using var connection = new SqlConnection(_connectionString);
        var recipes = (await connection.QueryAsync<Recipe>(
            "SELECT Id, Name, Type, RecipeText, Created, Updated FROM Recipes WHERE Type = @Type ORDER BY Updated DESC",
            new { Type = type })).ToList();
        
        foreach (var recipe in recipes)
        {
            recipe.Pictures = (await _pictureRepository.GetByRecipeIdAsync(recipe.Id)).ToList();
        }
        
        return recipes;
    }

    public async Task<IEnumerable<Recipe>> SearchAsync(string searchTerm)
    {
        using var connection = new SqlConnection(_connectionString);
        var recipes = (await connection.QueryAsync<Recipe>(
            "SELECT Id, Name, Type, RecipeText, Created, Updated FROM Recipes WHERE Name LIKE @SearchTerm ORDER BY Updated DESC",
            new { SearchTerm = $"%{searchTerm}%" })).ToList();
        
        foreach (var recipe in recipes)
        {
            recipe.Pictures = (await _pictureRepository.GetByRecipeIdAsync(recipe.Id)).ToList();
        }
        
        return recipes;
    }

    public async Task<Guid> CreateAsync(Recipe recipe)
    {
        using var connection = new SqlConnection(_connectionString);
        recipe.Id = Guid.NewGuid();
        recipe.Created = DateTime.UtcNow;
        recipe.Updated = DateTime.UtcNow;
        
        await connection.ExecuteAsync(
            "INSERT INTO Recipes (Id, Name, Type, RecipeText, Created, Updated) VALUES (@Id, @Name, @Type, @RecipeText, @Created, @Updated)",
            recipe);
        
        return recipe.Id;
    }

    public async Task<bool> UpdateAsync(Recipe recipe)
    {
        using var connection = new SqlConnection(_connectionString);
        recipe.Updated = DateTime.UtcNow;
        
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE Recipes SET Name = @Name, Type = @Type, RecipeText = @RecipeText, Updated = @Updated WHERE Id = @Id",
            recipe);
        
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM Recipes WHERE Id = @Id",
            new { Id = id });
        
        return rowsAffected > 0;
    }
}
