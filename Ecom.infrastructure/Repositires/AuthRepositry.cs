using Ecom.core.DTO;
using Ecom.core.Entities;
using Ecom.core.Interfaces;
using Ecom.core.Services;
using Ecom.core.Sharing;
using Ecom.infrastructure.Repositires.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositires
{
    class AuthRepositry:IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<AppUser> signManager;
        private readonly IGenerateToken generateToken;
        public AuthRepositry(UserManager<AppUser> userManager,IEmailService emailService,SignInManager<AppUser> signManager,IGenerateToken generateToken)
        {
            this.userManager = userManager;
            _emailService = emailService;
            this.signManager = signManager;
            this.generateToken = generateToken;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO) 
        {
            if (registerDTO == null) 
            {
                return null;
            }
            //check if username exist
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null) 
            {
                return "this username is already registered";
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null) 
            {
                return "this email is already registered";
            }
            AppUser user = new AppUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                DispalyName = registerDTO.DisplayName
            };
            //create user and save password in hash form
            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return result.Errors.ToList()[0].Description;
            }
            string token=await userManager.GenerateEmailConfirmationTokenAsync(user);
            SendEmail(user.Email, token, "active", "ActiveEmail", "Please active your Email click on button to active");
            return "Done";
        }

        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDTO(email, "ssaas656654@gmail.com", subject, EmailStringBody.Send(email, code, component, message));
            await _emailService.SendEmail(result); // Use the injected _emailService instance
        }

        public async Task<string> LoginAsync(LoginDTO login)
        {
            if (login == null)
                return null;
            var user = await userManager.FindByEmailAsync(login.Email);
            if (!user.EmailConfirmed)
            {
                string token =await userManager.GenerateEmailConfirmationTokenAsync(user);
                await SendEmail(user.Email, token, "active", "ActiveEmail", "Please active your Email click on button to active");
                return "Please Confirm Your email first, we send active to your Email";
            }

            var result = await signManager.CheckPasswordSignInAsync(user,login.Password,true);
            if (result.Succeeded)
            {
                return generateToken.GetAndCreateToken(user);
            }
             
            return "Please check your email and password, something went wrong";
        }

        public async Task<bool> SendEmailForForgetPassword(string email) 
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return false;
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await SendEmail(user.Email, token, "resetPassword", "Reset Password", "Please click on button to reset your password");
            return true;
        }

        public async Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user is null)
                return null;
            var result = await userManager.ResetPasswordAsync(user,resetPasswordDTO.Token,resetPasswordDTO.Password);
            if (result.Succeeded)
                return "Password changed success";
            return result.Errors.ToList()[0].Description;

        }

        public async Task<bool> ActiveAccount(ActiveAccountDTO activeAccountDTO) 
        {
            var user = await userManager.FindByEmailAsync(activeAccountDTO.Email);
            if (user is null)
                return false;
            var result = await userManager.ConfirmEmailAsync(user, activeAccountDTO.Token);
            if(result.Succeeded)
                return true;
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActiveEmail", "Please active your Email click on button to active");
            return false;

        }
    }
}
