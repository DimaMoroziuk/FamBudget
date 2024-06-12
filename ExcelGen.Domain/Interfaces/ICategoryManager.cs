using ExcelGen.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Interfaces
{
    public interface ICategoryManager
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(string id);
        Task UpdateExistingCategory(string id, Category category);
        Task CreateNewCategory(Category category);
        Task DeleteCategoryById(IEnumerable<string> ids);

    }
}
