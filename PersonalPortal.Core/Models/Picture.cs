namespace PersonalPortal.Core.Models;

public class Picture
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
    public string Caption { get; set; } = string.Empty;
    public Guid? RecipeId { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
