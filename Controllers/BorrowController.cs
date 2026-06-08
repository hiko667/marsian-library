using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using marsian_library.Models;
using marsian_library.Services;

namespace marsian_library.Controllers
{
        [Authorize(Roles = "Employee, Admin")]
    public class BorrowController : Controller
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        // GET: Borrow
        public async Task<IActionResult> Index()
        {
            var borrows = await _borrowService.GetAllBorrowsAsync();
            return View(borrows);
        }

        // GET: Borrow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _borrowService.GetBorrowByIdAsync(id.Value);
            if (borrow == null)
            {
                return NotFound();
            }

            return View(borrow);
        }

        // GET: Borrow/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CopyId"] = new SelectList(await _borrowService.GetCopiesAsync(), "Id", "Id");
            ViewData["ReaderId"] = new SelectList(await _borrowService.GetReadersAsync(), "Id", "FirstName");
            return View();
        }

        // POST: Borrow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CopyId,ReaderId,BorrowDate,ExpectedReturnDate,ReturnDate,TimesExtended")] Borrow borrow)
        {
            if (ModelState.IsValid)
            {
                await _borrowService.CreateBorrowAsync(borrow);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CopyId"] = new SelectList(await _borrowService.GetCopiesAsync(), "Id", "Id", borrow.CopyId);
            ViewData["ReaderId"] = new SelectList(await _borrowService.GetReadersAsync(), "Id", "FirstName", borrow.ReaderId);
            return View(borrow);
        }

        // GET: Borrow/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _borrowService.GetBorrowByIdAsync(id.Value);
            if (borrow == null)
            {
                return NotFound();
            }
            ViewData["CopyId"] = new SelectList(await _borrowService.GetCopiesAsync(), "Id", "Id", borrow.CopyId);
            ViewData["ReaderId"] = new SelectList(await _borrowService.GetReadersAsync(), "Id", "FirstName", borrow.ReaderId);
            return View(borrow);
        }

        // POST: Borrow/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken￼￼￼]
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
                    await _borrowService.UpdateBorrowAsync(borrow);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _borrowService.BorrowExistsAsync(borrow.Id))
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
            ViewData["CopyId"] = new SelectList(await _borrowService.GetCopiesAsync(), "Id", "Id", borrow.CopyId);
            ViewData["ReaderId"] = new SelectList(await _borrowService.GetReadersAsync(), "Id", "FirstName", borrow.ReaderId);
            return View(borrow);
        }

        // GET: Borrow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrow = await _borrowService.GetBorrowByIdAsync(id.Value);
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
            var success = await _borrowService.ReturnBookAsync(id);
            if (!success)
            {
                return NotFound();
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
            await _borrowService.DeleteBorrowAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}