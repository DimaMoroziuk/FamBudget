using ExcelGen.Domain.DTOs;
using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExcelGen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomeController : ControllerBase
    {

        private IIncomeManager _incomeManager;

        public IncomeController(IIncomeManager incomeManager)
        {
            _incomeManager = incomeManager;
        }

        // GET: api/Discipline
        [HttpGet("GetIncomes")]
        public ActionResult<IEnumerable<IncomeDTO>> GetIncomes()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return new ActionResult<IEnumerable<IncomeDTO>>(_incomeManager.GetAllIncomes(userId));
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/Discipline/5
        [HttpGet("GetIncome")]
        public async Task<ActionResult<Income>> GetIncome(string id)
        {
            try
            {
                return new ActionResult<Income>(await _incomeManager.GetIncomeById(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT: api/Discipline/5
        [HttpPut("PutIncome")]
        public async Task<IActionResult> PutIncome(string id, Income income)
        {
            try
            {
                await _incomeManager.UpdateExistingIncome(id, income);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Discipline
        [HttpPost("PostIncome")]
        public async Task<ActionResult<Income>> PostIncome(Income income)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                income.AuthorId = userId;
                await _incomeManager.CreateNewIncome(income);
            }
            catch
            {
                return BadRequest();
            }
            return CreatedAtAction("PostIncome", new { id = income.Id }, income);
        }

        // DELETE: api/Discipline/5
        [HttpDelete("DeleteIncome")]
        public async Task<ActionResult<Income>> DeleteIncome(IEnumerable<string> ids)
        {
            try
            {
                await _incomeManager.DeleteIncomeById(ids);
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}