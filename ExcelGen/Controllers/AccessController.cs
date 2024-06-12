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
    public class AccessController : ControllerBase
    {

        private IAccessManager _accessManager;

        public AccessController(IAccessManager accessManager)
        {
            _accessManager = accessManager;
        }

        [HttpGet("GetAccesses")]
        public async Task<ActionResult<IEnumerable<Access>>> GetAccesses()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return new ActionResult<IEnumerable<Access>>(await _accessManager.GetAllAccessesForCurrentUser(userId));
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: api/Discipline/5
        [HttpGet("GetAccess")]
        public async Task<ActionResult<Access>> GetAccess(string id)
        {
            try
            {
                return new ActionResult<Access>(await _accessManager.GetAccessById(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        // PUT: api/Discipline/5
        [HttpPut("PutAccess")]
        public async Task<IActionResult> PutAccess(string id, AccessDTO access)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _accessManager.UpdateExistingAccess(id, access, userId);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/Discipline
        [HttpPost("PostAccess")]
        public async Task<ActionResult<Access>> PostAccess(AccessDTO access)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _accessManager.CreateNewAccess(access, userId);
            }
            catch
            {
                return BadRequest();
            }
            return CreatedAtAction("PostAccess", access);
        }

        // DELETE: api/Discipline/5
        [HttpDelete("DeleteAccess")]
        public async Task<ActionResult> DeleteAccess(IEnumerable<string> ids)
        {
            try
            {
                await _accessManager.DeleteAccessById(ids);
            }
            catch
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
