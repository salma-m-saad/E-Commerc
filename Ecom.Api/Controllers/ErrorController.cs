using Ecom.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        /*
            Client Request
                  ↓
            Controller / Endpoint
                  ↓
            Status Code Error (404)
                  ↓
            UseStatusCodePagesWithReExecute
                  ↓
            /errors/404
                  ↓
            ErrorController
                  ↓
            ResponseAPI

         */
        [HttpGet]
        public IActionResult Error(int statusCode) 
        {
            return new ObjectResult(new ResponseAPI(statusCode));
        }

    }
}
