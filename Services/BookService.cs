using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;

namespace marsian_library.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetAllBooksAsync()
    {
        return await _context.Books
            .Select(b => new 
            {
                b.Guid, 
                b.Title,
                b.Isbn,
                Publisher = b.Publisher != null ? b.Publisher.Name : "Brak wydawcy",
                Genres = b.Genres.Select(g => g.Name).ToList(),
                Authors = b.Authors.Select(a => a.FirstName + " " + a.LastName).ToList(),
                Languages = b.Languages.Select(l => l.Name).ToList()
            })
            .ToListAsync();
    }

    public async Task<object?> GetBookByGuidAsync(string guid)
    {
        return await _context.Books
            .Where(b => b.Guid == guid) 
            .Select(b => new
            {
                b.Guid, 
                b.Title,
                b.Isbn,
                Publisher = b.Publisher != null ? b.Publisher.Name : "Brak wydawcy",
                Genres = b.Genres.Select(g => g.Name).ToList(),
                Authors = b.Authors.Select(a => a.FirstName + " " + a.LastName).ToList(),
                Languages = b.Languages.Select(l => l.Name).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<object>> GetAvailableBooksAsync()
    {
        return await _context.Books
            .Where(b => _context.Copies
                .Any(c => c.BookId == b.Id && 
                    !_context.Borrows.Any(borrow =>
                        borrow.CopyId == c.Id &&
                        borrow.ReturnDate == null))) 
            .Select(b => new 
            {
                b.Guid, 
                b.Title,
                b.Isbn,
                Publisher = b.Publisher != null ? b.Publisher.Name : "Brak wydawcy",
                Genres = b.Genres.Select(g => g.Name).ToList(),
                Authors = b.Authors.Select(a => a.FirstName + " " + a.LastName).ToList(),
                Languages = b.Languages.Select(l => l.Name).ToList()
            })
            .ToListAsync();
    }
}