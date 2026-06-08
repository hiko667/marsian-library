using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly AppDbContext _context;

        public LanguageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Language>> GetAllAsync()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<Language> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.Languages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateAsync(Language language)
        {
            _context.Add(language);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Language language)
        {
            _context.Update(language);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var language = await _context.Languages.FindAsync(id);
            if (language != null)
            {
                _context.Languages.Remove(language);
                await _context.SaveChangesAsync();
            }
        }

        public bool Exists(int id)
        {
            return _context.Languages.Any(e => e.Id == id);
        }
    }
}