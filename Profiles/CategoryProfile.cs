using AutoMapper;

namespace MyApi.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Models.Category, Data.Configurations.CategoryReturnDto>();
        CreateMap<Dtos.CategoryCreateDto, Models.Category>();
    }
    
}