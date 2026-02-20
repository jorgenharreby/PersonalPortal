using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public interface IChecklistRepository
{
    Task<Checklist?> GetByIdAsync(Guid id);
    Task<IEnumerable<Checklist>> GetAllAsync();
    Task<IEnumerable<Checklist>> GetLatestAsync(int count);
    Task<IEnumerable<Checklist>> GetByTypeAsync(string type);
    Task<IEnumerable<Checklist>> SearchAsync(string searchTerm);
    Task<Guid> CreateAsync(Checklist checklist);
    Task<bool> UpdateAsync(Checklist checklist);
    Task<bool> DeleteAsync(Guid id);
}
