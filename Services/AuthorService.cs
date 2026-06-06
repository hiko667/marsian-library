// Plik: Services/AuthorService.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly AppDbContext _context;

        public AuthorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAuthorAsync(Author author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }

        public bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}