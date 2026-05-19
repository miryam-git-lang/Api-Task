using AutoMapper;
using MyApi.Data.Configurations;
using MyApi.Dtos;
using MyApi.Extensions;
using MyApi.Models;

namespace MyApi.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile(IHttpContextAccessor accessor)
    {
        var request = accessor?.HttpContext?.Request;
        var uri = new UriBuilder
        {
            Scheme = request?.Scheme,
            Host = request?.Host.Host,
            Port = request?.Host.Port ?? 80
        };
        var baseUrl = uri.Uri.AbsoluteUri;
        CreateMap<ProductCreateDto,Product>();
        CreateMap<Product, ProductReturnDto>();
        
        CreateMap<CategoryCreateDto, Category>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.File.SaveFile("wwwroot/images")));
        CreateMap<Category, CategoryReturnDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src=> $"{baseUrl}images/{src.ImageUrl}"));
        CreateMap<CategoryUpdateDto, Category>();
        CreateMap<Category, CategoryInProductReturnDto>();
    }
}