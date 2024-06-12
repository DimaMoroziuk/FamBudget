using ExcelGen.Repository.Interfaces;
using ExcelGen.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelGen.Repository.Repositories
{
    public class AccessRepository : IAccessRepository
    {
        private readonly DatabaseContext _context;

        public AccessRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Access>> GetAccesses(string id)
        {
            var accesses = await _context.Access.Where(x => x.AuthorId == id).Include(x => x.Author).Include(x => x.AccessReciever).ToListAsync();

            if (accesses == null)
            {
                throw new Exception();
            }

            return accesses;
        }

        public async Task<Access> GetAccess(string id)
        {
            var access = _context.Access.Include(x => x.Author).Include(x => x.AccessReciever).FirstOrDefault(x => x.Id == id);

            if (access == null)
            {
                throw new Exception();
            }

            return access;
        }

        public async Task<List<Access>> GetSharerAccesses(string id)
        {
            var access = await _context.Access.Where(x => x.AccessRecieverId == id).ToListAsync();

            if (access == null)
            {
                throw new Exception();
            }

            return access;
        }

        public async Task UpdateExistingAccess(string id, Access access)
        {
            if (id != access.Id)
            {
                throw new Exception();
            }

            _context.Entry(access).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return;
        }


        public async Task CreateNewAccess(Access access)
        {
            _context.Access.Add(access);
            await _context.SaveChangesAsync();

            return;
        }

        public async Task DeleteAccessById(IEnumerable<string> ids)
        {
            var accesses = await _context.Access.Where(pur => ids.Contains(pur.Id)).ToListAsync();
            if (accesses == null)
            {
                throw new Exception();
            }

            _context.Access.RemoveRange(accesses);
            await _context.SaveChangesAsync();

            return;

        }
    }
}
