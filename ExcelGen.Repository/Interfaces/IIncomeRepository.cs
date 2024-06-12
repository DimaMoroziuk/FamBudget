using ExcelGen.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExcelGen.Repository.Interfaces
{
    public interface IIncomeRepository
    {
        Task<List<Income>> GetAllIncomes(List<string> userIds);
        Task<List<Income>> GetIncomesByYear(int year, List<string> userIds);
        Task<Income> GetIncomeById(string id);
        Task UpdateExistingIncome(string id, Income income);
        Task CreateNewIncome(Income income);
        Task DeleteIncomeById(IEnumerable<string> ids);
    }
}
