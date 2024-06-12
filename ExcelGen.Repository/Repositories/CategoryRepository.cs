using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelGen.Repository.Repositories
{
    public class CategoryRepository : ICategoryRepository
    { 
        private readonly DatabaseContext _context;

        public CategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategories()
        {
            var categories = await _context.Category.ToListAsync();

            if (categories == null)
            {
                throw new Exception();
            }

            return categories;
        }

        public async Task<Category> GetCategory(string id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                throw new Exception();
            }

            return category;
        }
        public async Task UpdateExistingCategory(string id, Category category)
        {
            if (id != category.Id)
            {
                throw new Exception();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return;
        }


        public async Task CreateNewCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task DeleteCategoryById(IEnumerable<string> ids)
        {
            var categories = await _context.Category.Where(pur => ids.Contains(pur.Id)).ToListAsync();
            if (categories == null)
            {
                throw new Exception();
            }

            _context.Category.RemoveRange(categories);
            await _context.SaveChangesAsync();

            return;

        }
    }
}
