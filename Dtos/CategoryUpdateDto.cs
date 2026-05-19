namespace MyApi.Dtos;

public class CategoryUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public IFormFile? File { get; set; }
}