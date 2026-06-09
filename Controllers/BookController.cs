using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly int PageSize = 9;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BookController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Book/Index
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

    // GET: Book/Details/5
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

        // Pobierz aktualne wypożyczenia dla tych egzemplarzy
        var currentBorrows = await _context.Borrows
            .Include(b => b.Reader)
            .Where(b => b.ReturnDate == null && copies.Select(c => c.Id).Contains(b.CopyId))
            .ToListAsync();

        ViewBag.Copies = copies;
        ViewBag.CopyBorrowers = currentBorrows
            .Where(b => b.Reader != null)
            .ToDictionary(b => b.CopyId, b => b.Reader!);

        // Jeśli wybrano konkretny egzemplarz, przekaż do wypożyczenia
        if (copyId.HasValue)
        {
            TempData["CopyId"] = copyId.Value;
            return RedirectToAction("Borrow", new { copyId = copyId.Value });
        }

        return View(book);
    }

    // GET: Book/Create
    [Authorize(Roles = "Employee, Admin")]
    public async Task<IActionResult> Create()

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
    [Authorize(Roles = "Employee, Admin")]
    public async Task<IActionResult> Create(Book book, int[]? selectedAuthors, int[]? selectedGenres,
        int[]? selectedLanguages, List<DeptCopyInput>? departmentCopies, IFormFile? coverFile)
    {
        // Usuń walidację dla kolekcji
        ModelState.Remove("Authors");
        ModelState.Remove("Genres");
        ModelState.Remove("Languages");

        var selectedDepartments = departmentCopies?
            .Where(dc => dc != null && dc.DeptId > 0 && dc.NumberOfCopies > 0)
            .ToList() ?? new List<DeptCopyInput>();

        if (!selectedDepartments.Any())
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

                // Cover image
                if (coverFile != null && coverFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(coverFile.FileName).ToLower();
                    
                    if (!allowedExtensions.Contains(extension))
                    {
                        throw new Exception("Invalid image format. Only JPG, JPEG and PNG are allowed.");
                    }

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "covers");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = book.Id + extension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save image
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await coverFile.CopyToAsync(fileStream);
                    }
                }

                // Pobierz stan "Available" (zakładając, że istnieje)
                var availableState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Available");
                if (availableState == null)
                {
                    throw new Exception("State 'Available' not found in database. Please ensure States table contains 'Available'.");
                }

                // Utwórz egzemplarze dla każdego wybranego departamentu
                foreach (var deptCopy in selectedDepartments)
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

                TempData["Success"] = $"Book '{book.Title}' created successfully with {selectedDepartments.Sum(dc => dc.NumberOfCopies)} copies across selected departments.";
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
    [Authorize(Roles = "Employee,Admin")]
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
    [Authorize(Roles = "Employee,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Book book, int[]? selectedAuthors, int[]? selectedGenres,
        int[]? selectedLanguages, Dictionary<int, int> addCopies, Dictionary<int, int> removeCopies, IFormFile? coverFile)
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

                // Cover image edit
                if (coverFile != null && coverFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var extension = Path.GetExtension(coverFile.FileName).ToLower();
                    
                    if (allowedExtensions.Contains(extension))
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "covers");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        // if the old cover had a different extension
                        foreach (var ext in allowedExtensions)
                        {
                            var oldFile = Path.Combine(uploadsFolder, bookToUpdate.Id + ext);
                            if (System.IO.File.Exists(oldFile)) System.IO.File.Delete(oldFile);
                        }

                        string filePath = Path.Combine(uploadsFolder, bookToUpdate.Id + extension);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await coverFile.CopyToAsync(fileStream);
                        }
                    }
                }

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
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Publisher)
            .Include(b => b.Authors)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null) return NotFound();

        return View(book);
    }

    // POST: Book/Delete/5
    [HttpPost, ActionName("Delete")]

    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            string[] extensions = { ".jpg", ".jpeg", ".png" };
            
            foreach (var ext in extensions)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "covers", id + ext);
                
                // Sprawdzamy, czy plik fizycznie istnieje na dysku serwera
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                        break;
                    }
                    catch (IOException ex)
                    {
                        ModelState.AddModelError("", $"Could not delete cover file: {ex.Message}");
                    }
                }
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Book and its cover image were successfully deleted.";
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Book/Borrow/5
    [Authorize(Roles = "Reader,Employee,Admin")]
    public async Task<IActionResult> Borrow(int? copyId)
    {
        if (copyId == null) return NotFound();

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["Error"] = "Only authenticated users can borrow books.";
            return RedirectToAction("Index");
        }

        var copy = await _context.Copies
            .Include(c => c.Book)
            .Include(c => c.Dept)
            .FirstOrDefaultAsync(c => c.Id == copyId);

        if (copy == null) return NotFound();

        // Sprawdź czy egzemplarz jest dostępny
        var availableState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Available");
        if (copy.StateId != availableState?.Id)
        {
            TempData["Error"] = "This copy is not available for borrowing.";
            return RedirectToAction("Index");
        }

        var borrow = new Borrow
        {
            CopyId = copy.Id,
            BorrowDate = DateTime.Now,
            ExpectedReturnDate = DateTime.Now.AddDays(14), // Domyślne 14 dni dla czytelnika
            TimesExtended = 0
        };

        var isStaff = User.IsInRole("Employee") || User.IsInRole("Admin");

        if (User.IsInRole("Reader"))
        {
            if (currentUser.ReaderId == null)
            {
                TempData["Error"] = "Your user account is not linked to a reader profile.";
                return RedirectToAction("Index");
            }

            borrow.ReaderId = currentUser.ReaderId.Value;
        }
        else if (isStaff)
        {
            ViewBag.ReaderId = new SelectList(_context.Readers, "Id", "FullName");
        }

        ViewBag.CopyInfo = $"{copy.Book?.Title} - Copy #{copy.Id}";
        ViewBag.IsStaff = isStaff;

        return View(borrow);
    }

    // POST: Book/Borrow
    [Authorize(Roles = "Reader,Employee,Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(Borrow borrow)
    {
        ModelState.Remove("Copy");
        ModelState.Remove("Reader");

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            TempData["Error"] = "Only authenticated users can borrow books.";
            return RedirectToAction("Index");
        }

        var isStaff = User.IsInRole("Employee") || User.IsInRole("Admin");

        if (User.IsInRole("Reader"))
        {
            if (currentUser.ReaderId == null)
            {
                TempData["Error"] = "Your user account is not linked to a reader profile.";
                return RedirectToAction("Index");
            }

            borrow.ReaderId = currentUser.ReaderId.Value;
            // Czytelnik nie może zmienić daty zwrotu - ustaw domyślną
            borrow.ExpectedReturnDate = DateTime.Now.AddDays(14);
        }
        else if (isStaff)
        {
            if (borrow.ReaderId <= 0 || !await _context.Readers.AnyAsync(r => r.Id == borrow.ReaderId))
            {
                ModelState.AddModelError("ReaderId", "Please select a valid reader.");
            }

            // Pracownik może ustawić własną datę zwrotu
            if (borrow.ExpectedReturnDate == default)
            {
                borrow.ExpectedReturnDate = DateTime.Now.AddDays(14);
            }
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Sprawdź ponownie dostępność egzemplarza
                var copy = await _context.Copies
                    .Include(c => c.State)
                    .FirstOrDefaultAsync(c => c.Id == borrow.CopyId);

                if (copy == null)
                {
                    TempData["Error"] = "Copy not found.";
                    return RedirectToAction("Index");
                }

                var availableState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Available");
                if (copy.State?.Name != "Available")
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

                // Ustaw datę wypożyczenia
                borrow.BorrowDate = DateTime.Now;

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

        // Jeśli błąd, przygotuj ponownie widok
        if (isStaff)
        {
            ViewBag.ReaderId = new SelectList(_context.Readers, "Id", "FullName", borrow.ReaderId);
        }

        var copyInfo = await _context.Copies
            .Include(c => c.Book)
            .FirstOrDefaultAsync(c => c.Id == borrow.CopyId);
        ViewBag.CopyInfo = $"{copyInfo?.Book?.Title} - Copy #{borrow.CopyId}";
        ViewBag.IsStaff = isStaff;

        return View(borrow);
    }

    // POST: Book/Return
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

    // GET: Book/MyBorrowings
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

    // GET: Book/ResetFilters
    public IActionResult ResetFilters()
    {
        return RedirectToAction("Index");
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
