using ExcelGen.Domain.DTOs;
using ExcelGen.Domain.Interfaces;
using ExcelGen.Repository.AuthorizationData;
using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExcelGen.Domain.Managers
{
    public class AccessManager : IAccessManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IAccessRepository _accessRepository;

        public AccessManager(IAccessRepository accessRepository, UserManager<ApplicationUser> userManager)
        {
            _accessRepository = accessRepository;
            _userManager = userManager;
        }

        public Task<List<Access>> GetAllAccessesForCurrentUser(string id)
        {
            return _accessRepository.GetAccesses(id);
        }
        public Task<Access> GetAccessById(string id)
        {
            return _accessRepository.GetAccess(id);
        }
        public async Task CreateNewAccess(AccessDTO accessDTO, string userId)
        {
            var user = await _userManager.FindByEmailAsync(accessDTO.Email);
            if (user != null)
            {
                var access = new Access();
                access.AccessType = accessDTO.AccessType;
                access.AuthorId = userId;
                access.AccessRecieverId = user.Id;

                await _accessRepository.CreateNewAccess(access);
            }

        }
        public async Task UpdateExistingAccess(string id, AccessDTO accessDTO, string userId)
        {
            var user = await _userManager.FindByEmailAsync(accessDTO.Email);
            if (user != null)
            {
                var access = new Access();
                access.AccessType = accessDTO.AccessType;
                access.AccessRecieverId = user.Id;

                await _accessRepository.UpdateExistingAccess(id, access);
            }
        }
        public Task DeleteAccessById(IEnumerable<string> ids)
        {
            return _accessRepository.DeleteAccessById(ids);
        }
    }
}
