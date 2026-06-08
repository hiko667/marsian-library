using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly AppDbContext _context;

        public PublisherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> GetByIdAsync(int id)
        {
            return await _context.Publishers.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateAsync(Publisher publisher)
        {
            _context.Add(publisher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Publisher publisher)
        {
            _context.Update(publisher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Publishers.AnyAsync(e => e.Id == id);
        }
    }
}