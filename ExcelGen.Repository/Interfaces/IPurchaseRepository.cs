using ExcelGen.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExcelGen.Repository.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAllPurchases(List<string> userIds);
        Task<List<Purchase>> GetPurchasesByYear(int year, List<string> userIds);
        Task<Purchase> GetPurchaseById(string id);
        Task UpdateExistingPurchase(string id, Purchase purchase);
        Task CreateNewPurchase(Purchase purchase);
        Task DeletePurchaseById(IEnumerable<string> ids);
    }
}
