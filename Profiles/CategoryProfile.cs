using AutoMapper;
using MyApi.Models;
using MyApi.Data.Configurations;
using MyApi.Dtos;
namespace MyApi.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryReturnDto>();
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryPatchDto, Category>();
        CreateMap<CategoryPatchDto, Category>();
    }
    
}