using ExcelGen.Domain.DTOs;
using ExcelGen.Domain.Enums;
using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Managers
{
    public class PurchaseManager : IPurchaseManager
    {
        private IPurchaseRepository _purchaseRepository;
        private IAccessRepository _accessRepository;

        public PurchaseManager(IPurchaseRepository purchaseRepository, IAccessRepository accessRepository)
        {
            _purchaseRepository = purchaseRepository;
            _accessRepository = accessRepository;
        }

        public List<PurchaseDTO> GetAllPurchases(string userId)
        {
            var sharerAccesses = _accessRepository.GetSharerAccesses(userId).Result;
            var sharerUserIds = sharerAccesses.Select(x => x.AuthorId).ToList();
            sharerUserIds.Add(userId);

            var purchases = _purchaseRepository.GetAllPurchases(sharerUserIds).Result;

            var purchaseDTOs = new List<PurchaseDTO>();

            foreach (var purchase in purchases.Where(x => x.AuthorId == userId))
            {
                var purchaseDTO = new PurchaseDTO(purchase);
                purchaseDTO.AccessType = (int)eAccessType.Edit;

                purchaseDTOs.Add(purchaseDTO);
            }

            foreach (var access in sharerAccesses)
            {
                var currentAccessPurchases = purchases.Where(x => x.AuthorId == access.AuthorId).Select(x => new PurchaseDTO(x));

                foreach (var purchase in currentAccessPurchases)
                {
                    purchase.AccessType = access.AccessType;
                }

                purchaseDTOs.AddRange(currentAccessPurchases);
            }

            return purchaseDTOs;
        }
        public Task<Purchase> GetPurchaseById(string id)
        {
            return _purchaseRepository.GetPurchaseById(id);
        }
        public Task UpdateExistingPurchase(string id, Purchase purchase)
        {
            return _purchaseRepository.UpdateExistingPurchase(id, purchase);
        }
        public Task CreateNewPurchase(Purchase purchase, string userId)
        {
            purchase.AuthorId = userId;
            return _purchaseRepository.CreateNewPurchase(purchase);
        }
        public Task DeletePurchaseById(IEnumerable<string> ids)
        {
            return _purchaseRepository.DeletePurchaseById(ids);
        }
    }
}
