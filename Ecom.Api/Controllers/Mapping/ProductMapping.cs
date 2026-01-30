using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entities.Product;

namespace Ecom.Api.Controllers.Mapping
{
    public class ProductMapping: Profile
    {
        public ProductMapping()
        {

            CreateMap<Product,ProductDTO>()
                //for Category Name in ProductDTO need to map from Category.Name in Product entity
                .ForMember(p=>p.CategoryName,
                op=>op.MapFrom(p=>p.Category.Name)).ReverseMap();

            CreateMap<Photo, PhotoDTO >().ReverseMap();

            CreateMap<AddProductDTO, Product>()
            .ForMember(p=>p.Photos,op=>op.Ignore())
            .ReverseMap();
        }
    }
}
