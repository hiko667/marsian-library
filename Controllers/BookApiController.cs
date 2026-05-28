using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace marsian_library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public BookApiController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _context.Books
            .Select(b => new 
            {
                b.Id,
                b.Title,
                b.Isbn,
                Publisher = b.Publisher != null ? b.Publisher.Name : "Brak wydawcy",
                Genres = b.Genres.Select(g => g.Name).ToList(),
                Authors = b.Authors.Select(a => a.FirstName + " " + a.LastName).ToList(),
                Languages = b.Languages.Select(l => l.Name).ToList()
            })
            .ToListAsync();

        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _context.Books
            .Where(b => b.Id == id)
            .Select(b => new
            {
                b.Id,
                b.Title,
                b.Isbn,
                Publisher = b.Publisher != null ? b.Publisher.Name : "Brak wydawcy",
                Genres = b.Genres.Select(g => g.Name).ToList(),
                Authors = b.Authors.Select(a => a.FirstName + " " + a.LastName).ToList(),
                Languages = b.Languages.Select(l => l.Name).ToList()
            })
            .FirstOrDefaultAsync();
        
        if (book == null)
        {
            return NotFound(new { message = $"Nie znaleziono książki o ID {id}" });
        }

        return Ok(book);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvaiable()
    {
        var books = await _context.Books
            .Where(b => _context.Copies
                .Any(c => c.BookId == b.Id &&
                    !_context.Borrows.Any(borrow =>
                        borrow.CopyId == c.Id &&
                        borrow.ReturnDate == null))) 
            .Select(b => new 
            {
                b.Id,
                b.Title,
                b.Isbn,
                Publisher = b.Publisher != null ? b.Publisher.Name : "Brak wydawcy",
                Genres = b.Genres.Select(g => g.Name).ToList(),
                Authors = b.Authors.Select(a => a.FirstName + " " + a.LastName).ToList(),
                Languages = b.Languages.Select(l => l.Name).ToList()
            })
            .ToListAsync();

        return Ok(books);
    }
}