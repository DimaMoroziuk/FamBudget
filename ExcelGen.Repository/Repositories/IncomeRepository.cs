using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelGen.Repository.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly DatabaseContext _context;

        public IncomeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public Task<List<Income>> GetIncomesByYear(int year, List<string> userIds)
        {
            var incomes = _context.Income.Where(inc => inc.Year == year && userIds.Contains(inc.AuthorId)).Include(en => en.Category).Include(x => x.Author).ToListAsync();

            if (incomes == null)
            {
                throw new Exception();
            }

            return incomes;
        }

        public async Task<List<Income>> GetAllIncomes(List<string> userIds)
        {
            return await _context.Income.Where(x => userIds.Contains(x.AuthorId)).Include(en => en.Category).Include(x => x.Author).ToListAsync();
        }

        public async Task<Income> GetIncomeById(string id)
        {
            var income = await _context.Income.FindAsync(id);

            if (income == null)
            {
                throw new Exception();
            }

            return income;
        }

        public async Task UpdateExistingIncome(string id, Income income)
        {
            if (id != income.Id)
            {
                throw new Exception();
            }

            _context.Entry(income).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                IncomeDTOExists(id);

                throw;
            }

            return;
        }

        public async Task CreateNewIncome(Income income)
        {
            income.AddedDate = DateTime.Now;
            _context.Income.Add(income);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task DeleteIncomeById(IEnumerable<string> ids)
        {
            var incomes= await _context.Income.Where(pur => ids.Contains(pur.Id)).ToListAsync();
            if (incomes == null)
            {
                throw new Exception();
            }

            _context.Income.RemoveRange(incomes);
            await _context.SaveChangesAsync();

            return;

        }

        #region Private Methods

        private void IncomeDTOExists(string id)
        {
            if (_context.Income.All(e => e.Id != id))
                throw new Exception();
        }

        #endregion
    }
}
