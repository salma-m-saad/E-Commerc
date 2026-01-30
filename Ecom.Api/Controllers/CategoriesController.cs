using AutoMapper;
using Ecom.Api.Helper;
using Ecom.core.DTO;
using Ecom.core.Entities.Product;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        [HttpGet ("get-all")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await unitOfWork.CategoryRepositry.GetAllAsync();
                if(categories==null)
                    return NotFound(new ResponseAPI(404));

                return Ok(categories);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetByID(int id) 
        {
            try 
            {
                var category = await unitOfWork.CategoryRepositry.GetByIdAsync(id);
                if(category==null)
                    return NotFound(new ResponseAPI(404,$"not found category id {id}"));
                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory(CategoryDTO categoryDTO)
        {
            try
            {
                var category = mapper.Map<Category>(categoryDTO);


                await unitOfWork.CategoryRepositry.AddAsync(category);
                return Ok(new ResponseAPI(201, "Item has been added"));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }

        }
        [HttpPut("update-category")]
        public async Task<IActionResult>UpdateCategory(UpdateCategoryDTO updateCategoryDTO) 
        {
            try
            {
                var category = mapper.Map<Category>(updateCategoryDTO);
                await unitOfWork.CategoryRepositry.UpdateAsync(category);
                return Ok(new ResponseAPI(200,"Item has been updated" ));

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
             
        }
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult>DeleteCategory(int id)
        {
            try
            {
                await unitOfWork.CategoryRepositry.DeleteAsync(id);
                return Ok(new ResponseAPI(200,"Item has been deleted" ));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
