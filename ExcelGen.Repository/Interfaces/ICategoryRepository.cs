using ExcelGen.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExcelGen.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(string id);
        Task DeleteCategoryById(IEnumerable<string> ids);
        Task UpdateExistingCategory(string id, Category category);
        Task CreateNewCategory(Category category);
    }
}
