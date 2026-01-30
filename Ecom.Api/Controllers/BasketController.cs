using AutoMapper;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.core.Entities;
using Ecom.Api.Helper;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpGet("get-basket-id/{id}")]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var basket = await unitOfWork.customerBasketRepositry.GetBasketAsync(id);
            if(basket is null)
                basket = new CustomerBasket();
            return Ok(basket);
        }


        [HttpPost("Update-basket")]
        public async Task<IActionResult> UpdateBasket([FromBody] CustomerBasket customerBasket)
        {
            var basket = await unitOfWork.customerBasketRepositry.UpdateBasketAsync(customerBasket);
            return Ok(basket);
        }


        [HttpDelete("delete-basket-id/{id}")]

        public async Task<IActionResult> DeleteBasketById(string id)
        {
            var result = await unitOfWork.customerBasketRepositry.DeleteBasketAsync(id);
            return(result)? Ok( new ResponseAPI( 200, "Item deleted")):BadRequest(new ResponseAPI(400));
        }

    }

}
