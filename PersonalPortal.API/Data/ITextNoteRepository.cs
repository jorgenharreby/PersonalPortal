using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public interface ITextNoteRepository
{
    Task<TextNote?> GetByIdAsync(Guid id);
    Task<IEnumerable<TextNote>> GetAllAsync();
    Task<IEnumerable<TextNote>> GetLatestAsync(int count);
    Task<IEnumerable<TextNote>> SearchAsync(string searchTerm);
    Task<Guid> CreateAsync(TextNote note);
    Task<bool> UpdateAsync(TextNote note);
    Task<bool> DeleteAsync(Guid id);
}
