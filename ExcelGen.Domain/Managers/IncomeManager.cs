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
    public class IncomeManager : IIncomeManager
    {
        private IIncomeRepository _incomeRepository;
        private IAccessRepository _accessRepository;

        public IncomeManager(IIncomeRepository incomeRepository, IAccessRepository accessRepository)
        {
            _incomeRepository = incomeRepository;
            _accessRepository = accessRepository;
        }

        public List<IncomeDTO> GetAllIncomes(string userId)
        {
            var sharerAccesses = _accessRepository.GetSharerAccesses(userId).Result;
            var sharerUserIds = sharerAccesses.Select(x => x.AuthorId).ToList();
            sharerUserIds.Add(userId);

            var incomes = _incomeRepository.GetAllIncomes(sharerUserIds).Result;

            var incomeDTOs = new List<IncomeDTO>();

            foreach (var income in incomes.Where(x => x.AuthorId == userId))
            {
                var incomeDTO = new IncomeDTO(income);
                incomeDTO.AccessType = (int)eAccessType.Edit;

                incomeDTOs.Add(incomeDTO);
            }

            foreach (var access in sharerAccesses)
            {
                var currentAccessIncomes = incomes.Where(x => x.AuthorId == access.AuthorId).Select(x => new IncomeDTO(x));

                foreach (var income in currentAccessIncomes)
                {
                    income.AccessType = access.AccessType;
                }

                incomeDTOs.AddRange(currentAccessIncomes);
            }

            return incomeDTOs;
        }
        public Task<Income> GetIncomeById(string id)
        {
            return _incomeRepository.GetIncomeById(id);
        }
        public Task UpdateExistingIncome(string id, Income income)
        {
            return _incomeRepository.UpdateExistingIncome(id, income);
        }
        public Task CreateNewIncome(Income income)
        {
            return _incomeRepository.CreateNewIncome(income);
        }
        public Task DeleteIncomeById(IEnumerable<string> ids)
        {
            return _incomeRepository.DeleteIncomeById(ids);
        }
    }
}
