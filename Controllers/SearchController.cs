using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Controllers;

public class SearchController : Controller
{
    private readonly AppDbContext _context;
    private readonly int PageSize = 3;

    public SearchController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Search/Index
    public async Task<IActionResult> Index(string searchString, int? page, int? copyId,
        int[]? selectedGenres, int[]? selectedLanguages, int[]? selectedPublishers)
    {
        // Obsługa wypożyczenia z widoku szczegółów
        if (copyId.HasValue)
        {
            TempData["CopyId"] = copyId.Value;
            return RedirectToAction("Borrow", new { copyId = copyId.Value });
        }

        // Pobierz wszystkie książki z relacjami
        var booksQuery = _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Languages)
            .AsQueryable();

        // Filtrowanie po tytule, ISBN lub autorze
        if (!string.IsNullOrEmpty(searchString))
        {
            searchString = searchString.ToLower();
            booksQuery = booksQuery.Where(b => 
                b.Title.ToLower().Contains(searchString) ||
                b.Isbn.Contains(searchString) ||
                b.Authors.Any(a => 
                    a.FirstName.ToLower().Contains(searchString) ||
                    a.LastName.ToLower().Contains(searchString))
            );
        }

        // Filtrowanie po gatunkach
        if (selectedGenres != null && selectedGenres.Any())
        {
            booksQuery = booksQuery.Where(b => 
                b.Genres.Any(g => selectedGenres.Contains(g.Id)));
        }

        // Filtrowanie po językach
        if (selectedLanguages != null && selectedLanguages.Any())
        {
            booksQuery = booksQuery.Where(b => 
                b.Languages.Any(l => selectedLanguages.Contains(l.Id)));
        }

        // Filtrowanie po wydawcach
        if (selectedPublishers != null && selectedPublishers.Any())
        {
            booksQuery = booksQuery.Where(b => 
                selectedPublishers.Contains(b.PublisherId));
        }

        // Paginacja
        int pageNumber = page ?? 1;
        int totalBooks = await booksQuery.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalBooks / PageSize);

        var books = await booksQuery
            .OrderBy(b => b.Title)
            .Skip((pageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        // Przekazanie danych do widoku
        ViewBag.CurrentPage = pageNumber;
        ViewBag.TotalPages = totalPages;
        ViewBag.SearchString = searchString;
        ViewBag.TotalBooks = totalBooks;
        ViewBag.SelectedGenres = selectedGenres ?? Array.Empty<int>();
        ViewBag.SelectedLanguages = selectedLanguages ?? Array.Empty<int>();
        ViewBag.SelectedPublishers = selectedPublishers ?? Array.Empty<int>();

        // Przygotuj listy dla filtrów
        ViewBag.Genres = new MultiSelectList(await _context.Genres.OrderBy(g => g.Name).ToListAsync(), "Id", "Name", selectedGenres);
        ViewBag.Languages = new MultiSelectList(await _context.Languages.OrderBy(l => l.Name).ToListAsync(), "Id", "Name", selectedLanguages);
        ViewBag.Publishers = new MultiSelectList(await _context.Publishers.OrderBy(p => p.Name).ToListAsync(), "Id", "Name", selectedPublishers);

        // Przygotowanie listy dostępnych egzemplarzy dla każdej książki
        var copiesDict = new Dictionary<int, List<Copy>>();
        foreach (var book in books)
        {
            var availableCopies = await _context.Copies
                .Include(c => c.State)
                .Include(c => c.Dept)
                .Where(c => c.BookId == book.Id && c.State.Name == "Available")
                .ToListAsync();
            copiesDict[book.Id] = availableCopies;
        }
        ViewBag.AvailableCopies = copiesDict;

        return View(books);
    }

    // GET: Search/Details/5
    public async Task<IActionResult> Details(int? id, int? copyId)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Languages)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null) return NotFound();

        // Pobierz wszystkie egzemplarze książki
        var copies = await _context.Copies
            .Include(c => c.State)
            .Include(c => c.Dept)
            .Where(c => c.BookId == id)
            .ToListAsync();

        ViewBag.Copies = copies;
        
        // Jeśli wybrano konkretny egzemplarz, przekaż do wypożyczenia
        if (copyId.HasValue)
        {
            TempData["CopyId"] = copyId.Value;
            return RedirectToAction("Borrow", new { copyId = copyId.Value });
        }

        return View(book);
    }

    // GET: Search/Borrow/5
    public async Task<IActionResult> Borrow(int? copyId)
    {
        if (copyId == null) return NotFound();

        var copy = await _context.Copies
            .Include(c => c.Book)
            .Include(c => c.Dept)
            .FirstOrDefaultAsync(c => c.Id == copyId);

        if (copy == null) return NotFound();

        // Sprawdź czy egzemplarz jest dostępny
        if (copy.StateId != 1) // Zakładając, że StateId=1 to "Available"
        {
            TempData["Error"] = "This copy is not available for borrowing.";
            return RedirectToAction("Index");
        }

        var borrow = new Borrow
        {
            CopyId = copy.Id,
            BorrowDate = DateTime.Now,
            ExpectedReturnDate = DateTime.Now.AddDays(14),
            TimesExtended = 0
        };

        // Przygotuj listę czytelników
        ViewBag.ReaderId = new SelectList(_context.Readers, "Id", "FullName");
        ViewBag.CopyInfo = $"{copy.Book?.Title} - Copy #{copy.Id}";

        return View(borrow);
    }

    // POST: Search/Borrow
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(Borrow borrow)
    {
        ModelState.Remove("Copy");
        ModelState.Remove("Reader");

        if (ModelState.IsValid)
        {
            try
            {
                // Sprawdź ponownie dostępność egzemplarza
                var copy = await _context.Copies
                    .Include(c => c.State)
                    .FirstOrDefaultAsync(c => c.Id == borrow.CopyId);

                if (copy == null || copy.State?.Name != "Available")
                {
                    TempData["Error"] = "This copy is no longer available.";
                    return RedirectToAction("Index");
                }

                // Zmień stan egzemplarza na "Borrowed"
                var borrowedState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Borrowed");
                if (borrowedState != null)
                {
                    copy.StateId = borrowedState.Id;
                    _context.Update(copy);
                }

                _context.Add(borrow);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = $"Book borrowed successfully! Expected return: {borrow.ExpectedReturnDate.ToShortDateString()}";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
            }
        }

        // Jeśli błąd, przygotuj ponownie listę czytelników
        ViewBag.ReaderId = new SelectList(_context.Readers, "Id", "FullName", borrow.ReaderId);
        var copyInfo = await _context.Copies
            .Include(c => c.Book)
            .FirstOrDefaultAsync(c => c.Id == borrow.CopyId);
        ViewBag.CopyInfo = $"{copyInfo?.Book?.Title} - Copy #{borrow.CopyId}";
        
        return View(borrow);
    }

    // POST: Search/Return
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int borrowId)
    {
        var borrow = await _context.Borrows
            .Include(b => b.Copy)
            .FirstOrDefaultAsync(b => b.Id == borrowId);

        if (borrow == null) return NotFound();

        if (borrow.ReturnDate == null)
        {
            borrow.ReturnDate = DateTime.Now;
            _context.Update(borrow);

            // Zmień stan egzemplarza z powrotem na "Available"
            var availableState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Available");
            if (availableState != null && borrow.Copy != null)
            {
                borrow.Copy.StateId = availableState.Id;
                _context.Update(borrow.Copy);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Book returned successfully!";
        }

        return RedirectToAction("Index");
    }

    // GET: Search/MyBorrowings
    public async Task<IActionResult> MyBorrowings(int? readerId)
    {
        if (readerId == null) return View(new List<Borrow>());

        var borrowings = await _context.Borrows
            .Include(b => b.Copy)
                .ThenInclude(c => c.Book)
            .Include(b => b.Copy)
                .ThenInclude(c => c.Dept)
            .Where(b => b.ReaderId == readerId && b.ReturnDate == null)
            .OrderBy(b => b.ExpectedReturnDate)
            .ToListAsync();

        return View(borrowings);
    }

    // GET: Search/ResetFilters
    public IActionResult ResetFilters()
    {
        return RedirectToAction("Index");
    }
}