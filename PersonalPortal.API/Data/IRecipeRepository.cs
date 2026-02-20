using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public interface IRecipeRepository
{
    Task<Recipe?> GetByIdAsync(Guid id);
    Task<IEnumerable<Recipe>> GetAllAsync();
    Task<IEnumerable<Recipe>> GetLatestAsync(int count);
    Task<IEnumerable<Recipe>> GetByTypeAsync(string type);
    Task<IEnumerable<Recipe>> SearchAsync(string searchTerm);
    Task<Guid> CreateAsync(Recipe recipe);
    Task<bool> UpdateAsync(Recipe recipe);
    Task<bool> DeleteAsync(Guid id);
}
