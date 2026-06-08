using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Services
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _context;

        public GenreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.Genres.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateAsync(Genre genre)
        {
            _context.Add(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, Genre genre)
        {
            if (id != genre.Id) return false;

            try
            {
                _context.Update(genre);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(genre.Id)) return false;
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
        }

        public bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}