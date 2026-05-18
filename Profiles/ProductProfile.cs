using AutoMapper;
using MyApi.Dtos;
using MyApi.Models;

namespace MyApi.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCreateDto, Product>();
        CreateMap<Product, ProductReturnDto>();
        CreateMap<ProductPatchDto, Product>();
    }
}