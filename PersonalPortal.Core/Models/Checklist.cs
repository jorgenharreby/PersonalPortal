namespace PersonalPortal.Core.Models;

public class Checklist
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public List<ChecklistItem> Items { get; set; } = new();
}

public class ChecklistItem
{
    public int Id { get; set; }
    public Guid ChecklistId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ItemGroup { get; set; }
}
