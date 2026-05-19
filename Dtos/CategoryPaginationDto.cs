using MyApi.Data.Configurations;

namespace MyApi.Dtos;

public class CategoryPaginationDto
{
    public int Page { get; set; }
    public int Take { get; set; }
    public int TotalCount { get; set; }
    
    public List<CategoryReturnDto> Categories { get; set; }
}