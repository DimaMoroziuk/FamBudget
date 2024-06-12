using ExcelGen.Domain.DTOs;
using ExcelGen.Repository.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Interfaces
{
    public interface IPurchaseManager
    {
        List<PurchaseDTO> GetAllPurchases(string userId);
        Task<Purchase> GetPurchaseById(string id);
        Task UpdateExistingPurchase(string id, Purchase purchase);
        Task CreateNewPurchase(Purchase purchase, string userId);
        Task DeletePurchaseById(IEnumerable<string> ids);
    }
}
