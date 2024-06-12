using ExcelGen.Domain.DTOs;
using ExcelGen.Repository.AuthorizationData;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Interfaces
{
    public interface IAccountManager
    {
        Task<string> Create(ApplicationUser dbUser, string userPassword);
        Task<bool> LoginUser(LoginDTO loginModel);
        Task LogoutUser();
        Task<ApplicationUser> GetUser(string id);
    }
}
