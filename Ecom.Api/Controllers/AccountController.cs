using AutoMapper;
using Ecom.Api.Helper;
using Ecom.core.DTO;
using Ecom.core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO) 
        {
            string result = await unitOfWork.Auth.RegisterAsync(registerDTO);
            if (result != "Done")
            {
                return BadRequest(new ResponseAPI(400,result));

            }
            return Ok(new ResponseAPI(200));

        }
        [HttpPost ("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO) 
        {
            string result = await unitOfWork.Auth.LoginAsync(loginDTO);
            if (result.StartsWith("Please"))
            {
                return BadRequest(new ResponseAPI(400, result));
            }

            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Domain = "localhost",
                IsEssential = true,
                Expires = DateTime.Now.AddDays(7)

            });
            return Ok(new ResponseAPI(200));
        }
        [HttpPost ("ActiveAccount")]
        public async Task<IActionResult> ActiveAccount(ActiveAccountDTO activeAccountDTO) 
        {
            var result = await unitOfWork.Auth.ActiveAccount(activeAccountDTO);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400, "Invalid token"));
        }
        [HttpGet ("ForgetPass")]
        public async Task<IActionResult> forgetpass(string email) 
        {
            var result = await unitOfWork.Auth.SendEmailForForgetPassword(email);

            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400, "Invalid token"));

        }
    }
}
