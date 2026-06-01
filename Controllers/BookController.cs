using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

using Microsoft.AspNetCore.Authorization;

namespace marsian_library.Controllers;

public class BookController : Controller
{
    private readonly AppDbContext _context;

    public BookController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Book
    public async Task<IActionResult> Index()
    {
        var books = _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Languages);
        return View(await books.ToListAsync());
    }

    // GET: Book/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Languages)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null) return NotFound();

        // Pobierz egzemplarze książki
        var copies = await _context.Copies
            .Include(c => c.Dept)
            .Include(c => c.State)
            .Where(c => c.BookId == id)
            .ToListAsync();

        ViewBag.Copies = copies;

        return View(book);
    }

    // GET: Book/Create

    [Authorize(Roles = "Employee")]
    public IActionResult Create()

    {
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
        ViewBag.Authors = new MultiSelectList(_context.Authors, "Id", "FullName");
        ViewBag.Genres = new MultiSelectList(_context.Genres, "Id", "Name");
        ViewBag.Languages = new MultiSelectList(_context.Languages, "Id", "Name");

        // Pobierz wszystkie departamenty
        var departments = await _context.Depts
            .Include(d => d.Address)
            .ToListAsync();
        ViewBag.Departments = departments;

        return View();
    }
    
    // POST: Book/Create
    [HttpPost]
    [ValidateAntiForgeryToken]

    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Create(Book book, int[]? selectedAuthors, int[]? selectedGenres, int[]? selectedLanguages)

    {
        // Usuń walidację dla kolekcji
        ModelState.Remove("Authors");
        ModelState.Remove("Genres");
        ModelState.Remove("Languages");

        // Sprawdź, czy wybrano przynajmniej jeden departament z liczbą kopii > 0
        if (departmentCopies == null || !departmentCopies.Any(dc => dc.NumberOfCopies > 0))
        {
            ModelState.AddModelError("", "Please select at least one department with at least one copy.");
            await PrepareCreateView();
            return View(book);
        }

        if (ModelState.IsValid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Dodaj autorów
                if (selectedAuthors != null && selectedAuthors.Any())
                {
                    book.Authors = await _context.Authors
                        .Where(a => selectedAuthors.Contains(a.Id))
                        .ToListAsync();
                }

                // Dodaj gatunki
                if (selectedGenres != null && selectedGenres.Any())
                {
                    book.Genres = await _context.Genres
                        .Where(g => selectedGenres.Contains(g.Id))
                        .ToListAsync();
                }

                // Dodaj języki
                if (selectedLanguages != null && selectedLanguages.Any())
                {
                    book.Languages = await _context.Languages
                        .Where(l => selectedLanguages.Contains(l.Id))
                        .ToListAsync();
                }

                // Zapisz książkę
                _context.Add(book);
                await _context.SaveChangesAsync();

                // Pobierz stan "Available" (zakładając, że istnieje)
                var availableState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Available");
                if (availableState == null)
                {
                    throw new Exception("State 'Available' not found in database. Please ensure States table contains 'Available'.");
                }

                // Utwórz egzemplarze dla każdego wybranego departamentu
                foreach (var deptCopy in departmentCopies.Where(dc => dc.NumberOfCopies > 0 && dc.DeptId > 0))
                {
                    for (int i = 0; i < deptCopy.NumberOfCopies; i++)
                    {
                        var copy = new Copy
                        {
                            BookId = book.Id,
                            DeptId = deptCopy.DeptId,
                            StateId = availableState.Id
                        };
                        _context.Copies.Add(copy);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = $"Book '{book.Title}' created successfully with {departmentCopies.Sum(dc => dc.NumberOfCopies)} copies across selected departments.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error creating book: {ex.Message}");
            }
        }

        await PrepareCreateView();
        return View(book);
    }

    // GET: Book/Edit/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Languages)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null) return NotFound();

        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
        ViewBag.Authors = new MultiSelectList(_context.Authors, "Id", "FullName", book.Authors.Select(a => a.Id));
        ViewBag.Genres = new MultiSelectList(_context.Genres, "Id", "Name", book.Genres.Select(g => g.Id));
        ViewBag.Languages = new MultiSelectList(_context.Languages, "Id", "Name", book.Languages.Select(l => l.Id));

        // Pobierz wszystkie departamenty
        var departments = await _context.Depts
            .Include(d => d.Address)
            .ToListAsync();
        ViewBag.Departments = departments;

        // Pobierz obecne liczby egzemplarzy w każdym departamencie
        var existingCopies = await _context.Copies
            .Where(c => c.BookId == id)
            .GroupBy(c => c.DeptId)
            .Select(g => new { DeptId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(k => k.DeptId, v => v.Count);

        ViewBag.ExistingCopies = existingCopies;

        return View(book);
    }

    // POST: Book/Edit/5
    [Authorize(Roles = "Employee")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book, int[]? selectedAuthors, int[]? selectedGenres,
        int[]? selectedLanguages, Dictionary<int, int> addCopies, Dictionary<int, int> removeCopies)
    {
        if (id != book.Id) return NotFound();

        ModelState.Remove("Authors");
        ModelState.Remove("Genres");
        ModelState.Remove("Languages");

        if (ModelState.IsValid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var bookToUpdate = await _context.Books
                    .Include(b => b.Authors)
                    .Include(b => b.Genres)
                    .Include(b => b.Languages)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (bookToUpdate == null) return NotFound();

                // Aktualizuj proste właściwości
                bookToUpdate.Title = book.Title;
                bookToUpdate.Isbn = book.Isbn;
                bookToUpdate.PublisherId = book.PublisherId;

                // Aktualizuj autorów
                bookToUpdate.Authors.Clear();
                if (selectedAuthors != null && selectedAuthors.Any())
                {
                    var authors = await _context.Authors
                        .Where(a => selectedAuthors.Contains(a.Id))
                        .ToListAsync();
                    foreach (var author in authors) bookToUpdate.Authors.Add(author);
                }

                // Aktualizuj gatunki
                bookToUpdate.Genres.Clear();
                if (selectedGenres != null && selectedGenres.Any())
                {
                    var genres = await _context.Genres
                        .Where(g => selectedGenres.Contains(g.Id))
                        .ToListAsync();
                    foreach (var genre in genres) bookToUpdate.Genres.Add(genre);
                }

                // Aktualizuj języki
                bookToUpdate.Languages.Clear();
                if (selectedLanguages != null && selectedLanguages.Any())
                {
                    var languages = await _context.Languages
                        .Where(l => selectedLanguages.Contains(l.Id))
                        .ToListAsync();
                    foreach (var language in languages) bookToUpdate.Languages.Add(language);
                }

                await _context.SaveChangesAsync();

                // Obsługa dodawania kopii
                var availableState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Available");
                if (availableState == null)
                {
                    throw new Exception("State 'Available' not found in database.");
                }

                if (addCopies != null)
                {
                    foreach (var add in addCopies.Where(a => a.Value > 0))
                    {
                        for (int i = 0; i < add.Value; i++)
                        {
                            var copy = new Copy
                            {
                                BookId = bookToUpdate.Id,
                                DeptId = add.Key,
                                StateId = availableState.Id
                            };
                            _context.Copies.Add(copy);
                        }
                    }
                }

                // Obsługa usuwania kopii (tylko dostępne, nie wypożyczone)
                if (removeCopies != null)
                {
                    foreach (var remove in removeCopies.Where(r => r.Value > 0))
                    {
                        var copiesToRemove = await _context.Copies
                            .Where(c => c.BookId == bookToUpdate.Id && c.DeptId == remove.Key && c.StateId == availableState.Id)
                            .Take(remove.Value)
                            .ToListAsync();

                        if (copiesToRemove.Count < remove.Value)
                        {
                            TempData["Warning"] = $"Only {copiesToRemove.Count} available copies were removed from department {remove.Key}. Some copies may be borrowed.";
                        }

                        _context.Copies.RemoveRange(copiesToRemove);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Book updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error updating book: {ex.Message}");
            }
        }

        // Przygotuj widok ponownie w przypadku błędu
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
        ViewBag.Authors = new MultiSelectList(_context.Authors, "Id", "FullName", selectedAuthors);
        ViewBag.Genres = new MultiSelectList(_context.Genres, "Id", "Name", selectedGenres);
        ViewBag.Languages = new MultiSelectList(_context.Languages, "Id", "Name", selectedLanguages);

        var departments = await _context.Depts.Include(d => d.Address).ToListAsync();
        ViewBag.Departments = departments;

        var existingCopies = await _context.Copies
            .Where(c => c.BookId == id)
            .GroupBy(c => c.DeptId)
            .Select(g => new { DeptId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(k => k.DeptId, v => v.Count);
        ViewBag.ExistingCopies = existingCopies;

        return View(book);
    }

    // GET: Book/Delete/5
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Languages)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null) return NotFound();

        return View(book);
    }

    // POST: Book/Delete/5
    [HttpPost, ActionName("Delete")]
    
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null) _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }

    private async Task PrepareCreateView()
    {
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
        ViewBag.Authors = new MultiSelectList(_context.Authors, "Id", "FullName");
        ViewBag.Genres = new MultiSelectList(_context.Genres, "Id", "Name");
        ViewBag.Languages = new MultiSelectList(_context.Languages, "Id", "Name");

        var departments = await _context.Depts
            .Include(d => d.Address)
            .ToListAsync();
        ViewBag.Departments = departments;
    }
}

// Klasa pomocnicza dla danych z formularza
public class DeptCopyInput
{
    public int DeptId { get; set; }
    public int NumberOfCopies { get; set; }
}