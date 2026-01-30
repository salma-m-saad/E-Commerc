using AutoMapper;
using Ecom.core.Entities;
using Ecom.core.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Ecom.core.Entities.Product;

namespace Ecom.Api.Controllers.AutoMapper
{
    public class CategoryMapping:Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO,Category>().ReverseMap();
            CreateMap<UpdateCategoryDTO,Category>().ReverseMap();
        }
    }
}
