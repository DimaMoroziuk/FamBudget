using ExcelGen.Domain.DTOs;
using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExcelGen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseController : ControllerBase
    {

        private IPurchaseManager _purchaseManager;

        public PurchaseController(IPurchaseManager purchaseManager)
        {
            _purchaseManager = purchaseManager;
        }

        // GET: api/Discipline
        [HttpGet("GetPurchases")]
        public ActionResult<IEnumerable<PurchaseDTO>> GetPurchases()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return new ActionResult<IEnumerable<PurchaseDTO>>( _purchaseManager.GetAllPurchases(userId));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        // GET: api/Discipline/5
        [HttpGet("GetPurchase")]
        public async Task<ActionResult<Purchase>> GetPurchase(string id)
        {
            try
            {
                return new ActionResult<Purchase>(await _purchaseManager.GetPurchaseById(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT: api/Discipline/5
        [HttpPut("PutPurchase")]
        public async Task<IActionResult> PutPurchase(string id, Purchase purchase)
        {
            try
            {
                await _purchaseManager.UpdateExistingPurchase(id, purchase);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Discipline
        [HttpPost("PostPurchase")]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _purchaseManager.CreateNewPurchase(purchase, userId);
            }
            catch
            {
                return BadRequest();
            }
            return CreatedAtAction("PostDisciplineDTO", new { id = purchase.Id }, purchase);
        }

        // DELETE: api/Discipline/5
        [HttpDelete("DeletePurchase")]
        public async Task<ActionResult<Purchase>> DeletePurchase(IEnumerable<string> ids)
        {
            try
            {
                await _purchaseManager.DeletePurchaseById(ids);
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}