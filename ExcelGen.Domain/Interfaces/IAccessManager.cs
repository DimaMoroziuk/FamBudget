using ExcelGen.Domain.DTOs;
using ExcelGen.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Interfaces
{
    public interface IAccessManager
    {
        Task<List<Access>> GetAllAccessesForCurrentUser(string id);
        Task<Access> GetAccessById(string id);
        Task CreateNewAccess(AccessDTO access, string userId);
        Task UpdateExistingAccess(string id, AccessDTO access, string userId);
        Task DeleteAccessById(IEnumerable<string> ids);
    }
}
