using ExcelGen.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelGen.Repository.Interfaces
{
    public interface IAccessRepository
    {
        Task<List<Access>> GetAccesses(string id);
        Task<Access> GetAccess(string id);
        Task DeleteAccessById(IEnumerable<string> ids);
        Task UpdateExistingAccess(string id, Access access);
        Task CreateNewAccess(Access access);
        Task<List<Access>> GetSharerAccesses(string id);
    }
}
