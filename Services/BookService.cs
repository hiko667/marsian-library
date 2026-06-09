using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using ClosedXML.Excel;

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

    public async Task<byte[]> GenerateExcelForUserBorrowsAsync(int userId)
    {
        var borrows = await _context.Borrows
            .Where(b => b.ReaderId == userId)
            .Select(b => new
            {
                BookTitle = b.Copy.Book.Title,
                BorrowDate = b.BorrowDate,
                ReturnDate = b.ReturnDate
            })
            .ToListAsync();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Wypożyczenia");

            worksheet.Cell(1, 1).Value = "Tytuł Książki";
            worksheet.Cell(1, 2).Value = "Data Wypożyczenia";
            worksheet.Cell(1, 3).Value = "Data Zwrotu / Status";

            worksheet.Range("A1:C1").Style.Font.Bold = true;

            int row = 2;
            foreach (var item in borrows)
            {
                worksheet.Cell(row, 1).Value = item.BookTitle;
                worksheet.Cell(row, 2).Value = item.BorrowDate.ToString("yyyy-MM-dd HH:mm");
                worksheet.Cell(row, 3).Value = item.ReturnDate.HasValue 
                    ? item.ReturnDate.Value.ToString("yyyy-MM-dd HH:mm") 
                    : "W trakcie wypożyczenia";
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}