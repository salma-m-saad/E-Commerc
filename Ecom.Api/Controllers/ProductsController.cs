using AutoMapper;
using Ecom.Api.Helper;
using Ecom.core.DTO;
using Ecom.core.Interfaces;
using Ecom.core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{

    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductParams productParams)
        {
            try
            {
                var products = await unitOfWork.ProductRepositry
                    .GetAllAsync(productParams);


                return Ok(new Pagination <ProductDTO>(productParams.PageNum,productParams.PageSize,products.TotalCount,products.products));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetProductByID(int id)
        {
            try
            {
                var product = await unitOfWork.ProductRepositry.GetByIdAsync(id, p => p.Category, p => p.Photos);
                var productDTO = mapper.Map<ProductDTO>(product);
                if (productDTO is null)
                    return NotFound(new ResponseAPI(404));
                return Ok(productDTO);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }



        [HttpPost("Add-Product")]
        public async Task<IActionResult> AddProduct(AddProductDTO productDTO) 
        {
            try 
            {
                await unitOfWork.ProductRepositry.AddAsync(productDTO);
                return Ok(new ResponseAPI(201, "Item has been added"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("Update-Product")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO updateProductDTO)
        {
            try
            {
                var isUpdated = await unitOfWork.ProductRepositry.UpdateAsync(updateProductDTO);
                if (!isUpdated)
                    return NotFound(new ResponseAPI(404, "Item not found"));
                return Ok(new ResponseAPI(200, "Item has been updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpDelete("Delete-Product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id) 
        {
            try 
            {
                var product = await unitOfWork.ProductRepositry.GetByIdAsync(id,p=>p.Photos,p=>p.Category);
                
                await unitOfWork.ProductRepositry.DeleteAsync(product);

                return Ok(new ResponseAPI(200, "Item has been deleted"));

            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
