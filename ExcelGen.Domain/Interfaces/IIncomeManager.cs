using ExcelGen.Domain.DTOs;
using ExcelGen.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Interfaces
{
    public interface IIncomeManager
    {
        List<IncomeDTO> GetAllIncomes(string userId);
        Task<Income> GetIncomeById(string id);
        Task UpdateExistingIncome(string id, Income income);
        Task CreateNewIncome(Income income);
        Task DeleteIncomeById(IEnumerable<string> ids);
    }
}
