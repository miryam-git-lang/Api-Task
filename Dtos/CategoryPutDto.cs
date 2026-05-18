namespace MyApi.Dtos;

public class CategoryPutDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}