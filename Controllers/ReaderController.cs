using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Controllers
{
    public class ReaderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly int PageSize = 3;

        public ReaderController(AppDbContext context)
        {
            _context = context;
        }


        // GET: Reader
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Index(string searchString, int? page)
        {


            // Pobierz wszystkich czytelników
            var readerQuery = _context.Readers
                .Include(r => r.Address)
                .Include(r => r.Borrows)
                .AsQueryable();

            // Filtrowanie po imieniu lub nazwisku
            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                readerQuery = readerQuery.Where(r =>
                    r.FirstName.ToLower().Contains(searchString) ||
                    r.LastName.ToLower().Contains(searchString));
            }


            // Paginacja
            int pageNumber = page ?? 1;
            int totalReaders = await readerQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalReaders / PageSize);

            var readers = await readerQuery
                .OrderBy(r => r.LastName)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Przekazanie danych do widoku
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;
            ViewBag.TotalBooks = totalReaders;

            return View(readers);
        }



        // GET: Reader/Details/5
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reader = await _context.Readers
                .Include(r => r.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

#pragma warning disable CS8602
            // Fetch current borrows
            var currentBorrows = await _context.Borrows
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                        .ThenInclude(b => b.Authors)
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Dept)
                .Where(b => b.ReaderId == reader.Id && b.ReturnDate == null)
                .OrderBy(b => b.ExpectedReturnDate)
                .Select(b => new BorrowInfo
                {
                    BorrowId = b.Id,
                    BookTitle = b.Copy!.Book!.Title,
                    Author = b.Copy.Book.Authors.FirstOrDefault() != null ? b.Copy.Book.Authors.FirstOrDefault()!.FullName : "Unknown",
                    CopyId = b.CopyId,
                    DeptId = b.Copy.Dept != null ? b.Copy.Dept.Id : 0,
                    BorrowDate = b.BorrowDate,
                    ExpectedReturnDate = b.ExpectedReturnDate,
                    ReturnDate = b.ReturnDate,
                    TimesExtended = b.TimesExtended,
                    ExtensionsRemaining = 3 - b.TimesExtended,
                    CanExtend = b.TimesExtended < 3
                })
                .ToListAsync();
#pragma warning restore CS8602

            ViewBag.CurrentBorrows = currentBorrows;

            return View(reader);
        }

        // GET: Reader/BorrowHistory/5
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> BorrowHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reader = await _context.Readers
                .Include(r => r.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

#pragma warning disable CS8602
            var borrowHistory = await _context.Borrows
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                        .ThenInclude(b => b.Authors)
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Dept)
                .Where(b => b.ReaderId == reader.Id && b.ReturnDate != null)
                .OrderByDescending(b => b.BorrowDate)
                .Select(b => new BorrowInfo
                {
                    BorrowId = b.Id,
                    BookTitle = b.Copy!.Book!.Title,
                    Author = b.Copy.Book.Authors.FirstOrDefault() != null ? b.Copy.Book.Authors.FirstOrDefault()!.FullName : "Unknown",
                    CopyId = b.CopyId,
                    DeptId = b.Copy.Dept != null ? b.Copy.Dept.Id : 0,
                    BorrowDate = b.BorrowDate,
                    ExpectedReturnDate = b.ExpectedReturnDate,
                    ReturnDate = b.ReturnDate,
                    TimesExtended = b.TimesExtended,
                    ExtensionsRemaining = 3 - b.TimesExtended,
                    CanExtend = b.TimesExtended < 3
                })
                .ToListAsync();
#pragma warning restore CS8602

            ViewBag.Reader = reader;
            ViewBag.BorrowHistory = borrowHistory;

            return View(reader);
        }

        // GET: Reader/Create
        [Authorize(Roles = "Employee, Admin")]
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Building");
            return View();
        }

        // POST: Reader/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Create([Bind("Id,AddressId,FirstName,LastName")] Reader reader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Building", reader.AddressId);
            return View(reader);
        }

        // GET: Reader/Edit/5
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reader = await _context.Readers.FindAsync(id);
            if (reader == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Building", reader.AddressId);
            return View(reader);
        }

        // POST: Reader/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AddressId,FirstName,LastName")] Reader reader)
        {
            if (id != reader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaderExists(reader.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Building", reader.AddressId);
            return View(reader);
        }

        // GET: Reader/Delete/5
        [Authorize(Roles = "Employee, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reader = await _context.Readers
                .Include(r => r.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        // POST: Reader/Delete/5
        [Authorize(Roles = "Employee, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader != null)
            {
                _context.Readers.Remove(reader);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReaderExists(int id)
        {
            return _context.Readers.Any(e => e.Id == id);
        }
    }

    public class BorrowInfo
    {
        public int BorrowId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int CopyId { get; set; }
        public int DeptId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int TimesExtended { get; set; }
        public int ExtensionsRemaining { get; set; }
        public bool CanExtend { get; set; }
    }
}
