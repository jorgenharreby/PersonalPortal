using Dapper;
using Microsoft.Data.SqlClient;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public class TextNoteRepository : ITextNoteRepository
{
    private readonly string _connectionString;

    public TextNoteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<TextNote?> GetByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<TextNote>(
            "SELECT Id, Name, Content, Created, Updated FROM TextNotes WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<IEnumerable<TextNote>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TextNote>(
            "SELECT Id, Name, Content, Created, Updated FROM TextNotes ORDER BY Updated DESC");
    }

    public async Task<IEnumerable<TextNote>> GetLatestAsync(int count)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TextNote>(
            "SELECT TOP (@Count) Id, Name, Content, Created, Updated FROM TextNotes ORDER BY Updated DESC",
            new { Count = count });
    }

    public async Task<IEnumerable<TextNote>> SearchAsync(string searchTerm)
    {
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<TextNote>(
            "SELECT Id, Name, Content, Created, Updated FROM TextNotes WHERE Name LIKE @SearchTerm ORDER BY Updated DESC",
            new { SearchTerm = $"%{searchTerm}%" });
    }

    public async Task<Guid> CreateAsync(TextNote note)
    {
        using var connection = new SqlConnection(_connectionString);
        note.Id = Guid.NewGuid();
        note.Created = DateTime.UtcNow;
        note.Updated = DateTime.UtcNow;
        
        await connection.ExecuteAsync(
            "INSERT INTO TextNotes (Id, Name, Content, Created, Updated) VALUES (@Id, @Name, @Content, @Created, @Updated)",
            note);
        
        return note.Id;
    }

    public async Task<bool> UpdateAsync(TextNote note)
    {
        using var connection = new SqlConnection(_connectionString);
        note.Updated = DateTime.UtcNow;
        
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE TextNotes SET Name = @Name, Content = @Content, Updated = @Updated WHERE Id = @Id",
            note);
        
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM TextNotes WHERE Id = @Id",
            new { Id = id });
        
        return rowsAffected > 0;
    }
}
