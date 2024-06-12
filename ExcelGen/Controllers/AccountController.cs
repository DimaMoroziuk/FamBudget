using ExcelGen.Domain.DTOs;
using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.AuthorizationData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExcelGen.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            try
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _accountManager.Create(user, model.Password);

                if (string.IsNullOrEmpty(result))
                {
                    // Handle successful registration
                    return new OkObjectResult("User Register Successfully ");
                }
                else
                {
                    return new BadRequestObjectResult("There was an error processing your request, please try again.");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
                throw;
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await _accountManager.LoginUser(model);

            return new OkObjectResult(result);
        }

        [HttpGet("GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                var result = await _accountManager.GetUser(userId);

                return new OkObjectResult(result);
            }
            return new BadRequestObjectResult("Login to get user data");
        }

        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _accountManager.LogoutUser();

            return new OkObjectResult("Logout succeded");
        }
    }
}