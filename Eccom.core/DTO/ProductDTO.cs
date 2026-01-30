using Ecom.core.Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.DTO
{
    public record ProductDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        public List<PhotoDTO> Photos { get; set; }
        public string CategoryName { get; set; }

    }

    public record ReturnProductDTO 
    {
        public List<ProductDTO> products { get; set; }
        public int TotalCount { get; set; }
    }
    public record PhotoDTO 
    {
        public string ImageName { get; set; }

        public int ProductID { get; set; }
    }
    public record AddProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public int CategoryID { get; set; }
        public IFormFileCollection Photo { get; set; }
    }
    public record UpdateProductDTO: AddProductDTO
    {
        public int ID { get; set; }
    }
}
