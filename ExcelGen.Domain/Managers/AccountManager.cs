using ExcelGen.Domain.DTOs;
using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.AuthorizationData;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> Create(ApplicationUser dbUser, string userPassword)
        {
            IdentityResult result = null;
            if (dbUser == null)
            {
                throw new ArgumentNullException("dbUser");
            }

            if (userPassword == null)
            {
                result = await _userManager.CreateAsync(dbUser);
            }
            else
            {
                result = await _userManager.CreateAsync(dbUser, userPassword);
            }

            //if (result.Succeeded)
            //{
            //    await _userManager.AddToRolesAsync(dbUser, new List<string>() { role, ProjectRoles.BaseUser });
            //    if (entityUser != null)
            //    {
            //        await _context.AddAsync(entityUser);
            //    }
            //    await _context.SaveChangesAsync();
            //    return string.Empty;
            //}
            return result.Errors.First().Description;
        }

        public async Task<bool> LoginUser(LoginDTO loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                return false;
            }

            var loginResult = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
            if (!loginResult.Succeeded)
            {
                return false;
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return true;
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user;
        }

        public async Task LogoutUser()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
