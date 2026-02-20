using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public interface IPictureRepository
{
    Task<Picture?> GetByIdAsync(Guid id);
    Task<IEnumerable<Picture>> GetAllAsync();
    Task<IEnumerable<Picture>> GetLatestAsync(int count);
    Task<IEnumerable<Picture>> GetByRecipeIdAsync(Guid recipeId);
    Task<Guid> CreateAsync(Picture picture);
    Task<bool> UpdateAsync(Picture picture);
    Task<bool> DeleteAsync(Guid id);
}
