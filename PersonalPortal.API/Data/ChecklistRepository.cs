using Dapper;
using Microsoft.Data.SqlClient;
using PersonalPortal.Core.Models;

namespace PersonalPortal.API.Data;

public class ChecklistRepository : IChecklistRepository
{
    private readonly string _connectionString;

    public ChecklistRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<Checklist?> GetByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        var checklist = await connection.QueryFirstOrDefaultAsync<Checklist>(
            "SELECT Id, Name, Type, Created, Updated FROM Checklists WHERE Id = @Id",
            new { Id = id });
        
        if (checklist != null)
        {
            checklist.Items = (await connection.QueryAsync<ChecklistItem>(
                "SELECT Id, ChecklistId, ItemName, Description, ItemGroup FROM ChecklistItems WHERE ChecklistId = @ChecklistId ORDER BY ItemGroup, ItemName",
                new { ChecklistId = id })).ToList();
        }
        
        return checklist;
    }

    public async Task<IEnumerable<Checklist>> GetAllAsync()
    {
        using var connection = new SqlConnection(_connectionString);
        var checklists = (await connection.QueryAsync<Checklist>(
            "SELECT Id, Name, Type, Created, Updated FROM Checklists ORDER BY Updated DESC")).ToList();
        
        foreach (var checklist in checklists)
        {
            checklist.Items = (await connection.QueryAsync<ChecklistItem>(
                "SELECT Id, ChecklistId, ItemName, Description, ItemGroup FROM ChecklistItems WHERE ChecklistId = @ChecklistId ORDER BY ItemGroup, ItemName",
                new { ChecklistId = checklist.Id })).ToList();
        }
        
        return checklists;
    }

    public async Task<IEnumerable<Checklist>> GetLatestAsync(int count)
    {
        using var connection = new SqlConnection(_connectionString);
        var checklists = (await connection.QueryAsync<Checklist>(
            "SELECT TOP (@Count) Id, Name, Type, Created, Updated FROM Checklists ORDER BY Updated DESC",
            new { Count = count })).ToList();
        
        foreach (var checklist in checklists)
        {
            checklist.Items = (await connection.QueryAsync<ChecklistItem>(
                "SELECT Id, ChecklistId, ItemName, Description, ItemGroup FROM ChecklistItems WHERE ChecklistId = @ChecklistId ORDER BY ItemGroup, ItemName",
                new { ChecklistId = checklist.Id })).ToList();
        }
        
        return checklists;
    }

    public async Task<IEnumerable<Checklist>> GetByTypeAsync(string type)
    {
        using var connection = new SqlConnection(_connectionString);
        var checklists = (await connection.QueryAsync<Checklist>(
            "SELECT Id, Name, Type, Created, Updated FROM Checklists WHERE Type = @Type ORDER BY Updated DESC",
            new { Type = type })).ToList();
        
        foreach (var checklist in checklists)
        {
            checklist.Items = (await connection.QueryAsync<ChecklistItem>(
                "SELECT Id, ChecklistId, ItemName, Description, ItemGroup FROM ChecklistItems WHERE ChecklistId = @ChecklistId ORDER BY ItemGroup, ItemName",
                new { ChecklistId = checklist.Id })).ToList();
        }
        
        return checklists;
    }

    public async Task<IEnumerable<Checklist>> SearchAsync(string searchTerm)
    {
        using var connection = new SqlConnection(_connectionString);
        var checklists = (await connection.QueryAsync<Checklist>(
            "SELECT Id, Name, Type, Created, Updated FROM Checklists WHERE Name LIKE @SearchTerm ORDER BY Updated DESC",
            new { SearchTerm = $"%{searchTerm}%" })).ToList();
        
        foreach (var checklist in checklists)
        {
            checklist.Items = (await connection.QueryAsync<ChecklistItem>(
                "SELECT Id, ChecklistId, ItemName, Description, ItemGroup FROM ChecklistItems WHERE ChecklistId = @ChecklistId ORDER BY ItemGroup, ItemName",
                new { ChecklistId = checklist.Id })).ToList();
        }
        
        return checklists;
    }

    public async Task<Guid> CreateAsync(Checklist checklist)
    {
        using var connection = new SqlConnection(_connectionString);
        checklist.Id = Guid.NewGuid();
        checklist.Created = DateTime.UtcNow;
        checklist.Updated = DateTime.UtcNow;
        
        await connection.ExecuteAsync(
            "INSERT INTO Checklists (Id, Name, Type, Created, Updated) VALUES (@Id, @Name, @Type, @Created, @Updated)",
            checklist);
        
        foreach (var item in checklist.Items)
        {
            item.ChecklistId = checklist.Id;
            await connection.ExecuteAsync(
                "INSERT INTO ChecklistItems (ChecklistId, ItemName, Description, ItemGroup) VALUES (@ChecklistId, @ItemName, @Description, @ItemGroup)",
                item);
        }
        
        return checklist.Id;
    }

    public async Task<bool> UpdateAsync(Checklist checklist)
    {
        using var connection = new SqlConnection(_connectionString);
        checklist.Updated = DateTime.UtcNow;
        
        var rowsAffected = await connection.ExecuteAsync(
            "UPDATE Checklists SET Name = @Name, Type = @Type, Updated = @Updated WHERE Id = @Id",
            checklist);
        
        await connection.ExecuteAsync(
            "DELETE FROM ChecklistItems WHERE ChecklistId = @ChecklistId",
            new { ChecklistId = checklist.Id });
        
        foreach (var item in checklist.Items)
        {
            item.ChecklistId = checklist.Id;
            await connection.ExecuteAsync(
                "INSERT INTO ChecklistItems (ChecklistId, ItemName, Description, ItemGroup) VALUES (@ChecklistId, @ItemName, @Description, @ItemGroup)",
                item);
        }
        
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM Checklists WHERE Id = @Id",
            new { Id = id });
        
        return rowsAffected > 0;
    }
}
