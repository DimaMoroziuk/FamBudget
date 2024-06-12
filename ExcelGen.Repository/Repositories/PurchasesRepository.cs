using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelGen.Repository.Repositories
{
    public class PurchasesRepository : IPurchaseRepository
    {
        private readonly DatabaseContext _context;

        public PurchasesRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllPurchases(List<string> userIds) 
        {
            return await _context.Purchase.Where(x => userIds.Contains(x.AuthorId)).Include(en => en.Category).Include(x => x.Author).ToListAsync();
        }

        public Task<List<Purchase>> GetPurchasesByYear(int year, List<string> userIds)
        {
            var purchases = _context.Purchase.Where(inc => inc.Year == year && userIds.Contains(inc.AuthorId)).Include(en => en.Category).Include(x => x.Author).ToListAsync();

            if (purchases == null)
            {
                throw new Exception();
            }

            return purchases;
        }


        public async Task<Purchase> GetPurchaseById(string id)
        {
            var purchase = await _context.Purchase.FindAsync(id);

            if (purchase == null)
            {
                throw new Exception();
            }

            return purchase;
        }

        public async Task UpdateExistingPurchase(string id, Purchase purchase) 
        {
            if (id != purchase.Id)
            {
                throw new Exception();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                PurchaseDTOExists(id);

                throw;
            }

            return;
        }

        public async Task CreateNewPurchase(Purchase purchase) 
        {
            purchase.AddedDate = DateTime.Now;
            _context.Purchase.Add(purchase);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task DeletePurchaseById(IEnumerable<string> ids) 
        {
            var purchases = await _context.Purchase.Where(pur => ids.Contains(pur.Id)).ToListAsync();
            if (purchases == null)
            {
                throw new Exception();
            }

            _context.Purchase.RemoveRange(purchases);
            await _context.SaveChangesAsync();

            return;

        }

        #region Private Methods

        private void PurchaseDTOExists(string id)
        {
            if (_context.Purchase.All(e => e.Id != id))
                throw new Exception();
        }

        #endregion
    }
}
