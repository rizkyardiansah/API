using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string> 
    {
        private readonly AccountRepository accountRepository;
        private readonly AccountRoleRepository accountRoleRepository;
        public IConfiguration _configuration;

        public AccountsController(AccountRepository accountRepository, AccountRoleRepository accountRoleRepository, IConfiguration configuration) : base(accountRepository) {
            this.accountRepository = accountRepository;
            this.accountRoleRepository = accountRoleRepository;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult Register(RegisterVM registerVM)
        {
            try
            {
                int result = accountRepository.Register(registerVM);

                if (result == -1)
                {
                    return BadRequest("Phone already used");
                }

                if (result == -2)
                {
                    return BadRequest("Email already used");
                }

                if (result == -3)
                {
                    return BadRequest("Gender can only be \"Male\" or \"Female\" ");
                }

                return Ok("Registration Success");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public ActionResult Login(LoginVM loginVM)
        {
            int result = accountRepository.Login(loginVM);

            if (result == -1)
            {
                return BadRequest("Account not Found");
            }

            if (result == -2)
            {
                return BadRequest("Wrong Password");
            }

            var accountRoleName =  accountRoleRepository.GetAccountRoleName(loginVM.Email);

            var claims = new List<Claim>
            {
                new Claim("Email", loginVM.Email)
            };

            foreach (string r in accountRoleName)
            {
                claims.Add(new Claim("roles", r));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);
            var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
            claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
            return Ok(new { status = 200, token = idtoken, message = "Login Success" });
        }

        [HttpPost("forgotpassword")]
        public ActionResult ForgotPassword(EmailVM forgotPasswordVM)
        {
            int result = accountRepository.ForgotPassword(forgotPasswordVM);
            if (result == -1)
            {
                return BadRequest("Account not Found");
            }

            return Ok("OTP Sent");
        }

        [HttpPost("changepassword")]
        public ActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            int result = accountRepository.ChangePassword(changePasswordVM);

            if (result == -1)
            {
                return BadRequest("Account not Found");
            }

            if (result == -2)
            {
                return BadRequest("New Password and Confirm Password is not Match");
            }

            if (result == -3)
            {
                return BadRequest("OTP is not Correct");
            }

            if (result == -4)
            {
                return BadRequest("OTP is Expired");
            }

            if (result == -5)
            {
                return BadRequest("OTP is Already Used");
            }

            return Ok("Password Changed");
        }
    }

}