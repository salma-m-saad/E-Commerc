using Ecom.core.Entities;
using Ecom.core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace Ecom.infrastructure.Repositires.Service
{
    public class GenerateTokenService : IGenerateToken
    {
        private readonly IConfiguration configuration;
        public GenerateTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GetAndCreateToken(AppUser user) 
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email)
            };
            var Security = configuration["Token:Secret"];
            var key = Encoding.ASCII.GetBytes(Security);
            SigningCredentials credentials=new SigningCredentials(new SymmetricSecurityKey( key),SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = configuration["Token:Issure"],
                SigningCredentials = credentials,
                NotBefore=DateTime.Now,

            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);

        }
    }
}
