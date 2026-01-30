using AutoMapper;
using Ecom.core.DTO;
using Ecom.core.Entities.Product;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.core.Sharing;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositires.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositires
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;
        private readonly IImageManagementServicecs imageManagementServicecs;
        public ProductRepositry(AppDbContext context, IImageManagementServicecs imageManagementServicecs, IMapper mapper) : base(context)
        {
            this.imageManagementServicecs = imageManagementServicecs;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> AddAsync(AddProductDTO productDTO)
        {
            if (productDTO == null)
                return false;
            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var ImagePath = await imageManagementServicecs.AddImageAsync(productDTO.Photo, productDTO.Name);
            var photo = ImagePath
                .Select(path => new Photo { ImageName = path, ProductID = product.ID }).ToList();

            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;


        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null) 
            {
                return false;
            }
            var product = await context.Products
                .Include(p=>p.Category)
                .Include(p=>p.Photos)
                .FirstOrDefaultAsync(p => p.ID == updateProductDTO.ID);
            if (product == null)
                return false;

            mapper.Map(updateProductDTO, product);

            var OldPhotos = await context.Photos.Where(ph=>ph.ProductID==updateProductDTO.ID).ToListAsync();
            //delete photos from directory
            foreach (var photo in OldPhotos)
            {
                imageManagementServicecs.DeleteImageAsync(photo.ImageName);
            }
            //delete photos from database
            context.Photos.RemoveRange(OldPhotos);


            //Add new image to directory & database
            var newPhotos = imageManagementServicecs.AddImageAsync(updateProductDTO.Photo, updateProductDTO.Name);
            var photoEntities = (await newPhotos)
                .Select(path => new Photo { ImageName = path, ProductID = updateProductDTO.ID }).ToList();

            await context.Photos.AddRangeAsync(photoEntities);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(Product product)
        {
            var photos =await context.Photos.Where(ph => ph.ProductID == product.ID).ToListAsync();

            foreach (var photo in photos)
            {
                imageManagementServicecs.DeleteImageAsync(photo.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<ReturnProductDTO> GetAllAsync(ProductParams productParams)
        {
            var products = context.Products
                .Include(p=>p.Category)
                .Include(p=>p.Photos)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var Search = productParams.Search.Split(' ');
                products = products.Where(p => Search.All(word =>
                p.Name.ToLower().Contains(word.ToLower())||
                p.Description.ToLower().Contains(word.ToLower())
                ));
            }

            if (productParams.CategoryID.HasValue) 
            {
                products=products.Where(p => p.CategoryID == productParams.CategoryID);
            }

            if (!string.IsNullOrEmpty(productParams.sort))
            {
                switch (productParams.sort)
                {
                    case "PriceAsc":
                        products=products.OrderBy(p => p.NewPrice);
                        break;
                    case "PriceDes":
                        products = products.OrderByDescending(p => p.NewPrice);
                        break;
                    default:
                        products = products.OrderBy(p => p.Name);
                        break;

                }

              
            }
            ReturnProductDTO returnProductDTO = new ReturnProductDTO();
            returnProductDTO.TotalCount = products.Count();


            products = products.Skip((productParams.PageSize) * (productParams.PageNum - 1)).Take(productParams.PageSize);
            returnProductDTO.products = mapper.Map<List<ProductDTO>>(products);
            return returnProductDTO; 

        }

      
    }
}
