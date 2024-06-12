using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Managers
{
    public class CategoryManager : ICategoryManager
    {
        private ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<List<Category>> GetAllCategories()
        {
            return _categoryRepository.GetCategories();
        }
        public Task<Category> GetCategoryById(string id)
        {
            return _categoryRepository.GetCategory(id);
        }
        public Task CreateNewCategory(Category category)
        {
            return _categoryRepository.CreateNewCategory(category);
        }
        public Task UpdateExistingCategory(string id, Category category)
        {
            return _categoryRepository.UpdateExistingCategory(id, category);
        }
        public Task DeleteCategoryById(IEnumerable<string> ids)
        {
            return _categoryRepository.DeleteCategoryById(ids);
        }
    }
}
