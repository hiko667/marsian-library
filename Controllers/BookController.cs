using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

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

        return View(book);
    }

    // GET: Book/Create
    public IActionResult Create()
    {
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
        ViewBag.Authors = new MultiSelectList(_context.Authors, "Id", "FullName");
        ViewBag.Genres = new MultiSelectList(_context.Genres, "Id", "Name");
        ViewBag.Languages = new MultiSelectList(_context.Languages, "Id", "Name");
        return View();
    }

    // POST: Book/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book, int[]? selectedAuthors, int[]? selectedGenres, int[]? selectedLanguages)
    {
        // Usuń walidację dla kolekcji
        ModelState.Remove("Authors");
        ModelState.Remove("Genres");
        ModelState.Remove("Languages");

        if (ModelState.IsValid)
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

            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Przygotuj listy w przypadku błędu
        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
        ViewBag.Authors = new MultiSelectList(_context.Authors, "Id", "FullName", selectedAuthors);
        ViewBag.Genres = new MultiSelectList(_context.Genres, "Id", "Name", selectedGenres);
        ViewBag.Languages = new MultiSelectList(_context.Languages, "Id", "Name", selectedLanguages);
        return View(book);
    }

    // GET: Book/Edit/5
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

        return View(book);
    }

    // POST: Book/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book, int[]? selectedAuthors, int[]? selectedGenres, int[]? selectedLanguages)
    {
        if (id != book.Id) return NotFound();

        ModelState.Remove("Authors");
        ModelState.Remove("Genres");
        ModelState.Remove("Languages");

        if (ModelState.IsValid)
        {
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
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id)) return NotFound();
                throw;
            }
        }

        ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
        ViewBag.Authors = new MultiSelectList(_context.Authors, "Id", "FullName", selectedAuthors);
        ViewBag.Genres = new MultiSelectList(_context.Genres, "Id", "Name", selectedGenres);
        ViewBag.Languages = new MultiSelectList(_context.Languages, "Id", "Name", selectedLanguages);
        return View(book);
    }

    // GET: Book/Delete/5
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
}