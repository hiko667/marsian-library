using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Controllers
{
        [Authorize(Roles = "Employee, Admin")]
    public class BorrowController : Controller
    {
        private readonly AppDbContext _context;

        public BorrowController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Borrow
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Borrows.Include(b => b.Copy)
                                               .Include(b => b.Reader)
                                               .Include(b => b.Copy.Book)
                                               .OrderByDescending(b => b.BorrowDate);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Borrow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.Copy)
                .Include(b => b.Reader)
                .Include(b => b.Copy.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // GET: Borrow/Create
        public IActionResult Create()
        {
            ViewData["CopyId"] = new SelectList(_context.Copies, "Id", "Id");
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "FirstName");
            return View();
        }

        // POST: Borrow/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CopyId,ReaderId,BorrowDate,ExpectedReturnDate,ReturnDate,TimesExtended")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(borrow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CopyId"] = new SelectList(_context.Copies, "Id", "Id", borrow.CopyId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "FirstName", borrow.ReaderId);
            return View(borrow);
        }

        // GET: Borrow/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return NotFound();
            }
            ViewData["CopyId"] = new SelectList(_context.Copies, "Id", "Id", borrow.CopyId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "FirstName", borrow.ReaderId);
            return View(borrow);
        }

        // POST: Borrow/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CopyId,ReaderId,BorrowDate,ExpectedReturnDate,ReturnDate,TimesExtended")] Borrow borrow)
        {
            if (id != borrow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowExists(borrow.Id))
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
            ViewData["CopyId"] = new SelectList(_context.Copies, "Id", "Id", borrow.CopyId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "FirstName", borrow.ReaderId);
            return View(borrow);
        }

        // GET: Borrow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _context.Borrows
                .Include(b => b.Copy)
                .Include(b => b.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // POST: Borrow/Return/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, int? readerId)
        {
            var borrow = await _context.Borrows
                .Include(b => b.Copy)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (borrow == null)
            {
                return NotFound();
            }

            if (borrow.ReturnDate == null)
            {
                borrow.ReturnDate = DateTime.Now;

                var availableState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Available");
                if (availableState == null)
                {
                    throw new InvalidOperationException("State 'Available' not found in database. Please ensure it exists.");
                }

                if (borrow.Copy != null)
                {
                    borrow.Copy.StateId = availableState.Id;
                    _context.Copies.Update(borrow.Copy);
                }

                _context.Borrows.Update(borrow);
                await _context.SaveChangesAsync();
            }

            if (readerId.HasValue)
            {
                return RedirectToAction("Details", "Reader", new { id = readerId.Value });
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Borrow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow != null)
            {
                _context.Borrows.Remove(borrow);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowExists(int id)
        {
            return _context.Borrows.Any(e => e.Id == id);
        }
    }
}
