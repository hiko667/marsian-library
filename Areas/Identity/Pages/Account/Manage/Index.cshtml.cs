using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public IndexModel(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public string? Email { get; set; }
        public Reader? Reader { get; set; }
        public IList<BorrowSummary> CurrentBorrows { get; set; } = new List<BorrowSummary>();
        public bool IsReader => Reader != null;

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return Challenge();
            }

            Email = user.Email;

            if (user.ReaderId != null)
            {
                Reader = await _context.Readers
                    .Include(r => r.Address)
                    .FirstOrDefaultAsync(r => r.Id == user.ReaderId.Value);

                if (Reader != null)
                {
#pragma warning disable CS8602
                    CurrentBorrows = await _context.Borrows
                        .Include(b => b.Copy)
                            .ThenInclude(c => c.Book)
                        .Include(b => b.Copy)
                            .ThenInclude(c => c.Book)
                                .ThenInclude(b => b.Authors)
                        .Include(b => b.Copy)
                            .ThenInclude(c => c.Dept)
                        .Where(b => b.ReaderId == Reader.Id && b.ReturnDate == null)
                        .OrderBy(b => b.ExpectedReturnDate)
                        .Select(b => new BorrowSummary
                        {
                            BorrowId = b.Id,
                            BookTitle = b.Copy!.Book!.Title,
                            Author = b.Copy.Book.Authors.FirstOrDefault() != null ? b.Copy.Book.Authors.FirstOrDefault()!.FullName : "Unknown",
                            CopyId = b.CopyId,
                            DeptId = b.Copy.Dept != null ? b.Copy.Dept.Id : 0,
                            BorrowDate = b.BorrowDate,
                            ExpectedReturnDate = b.ExpectedReturnDate,
                            TimesExtended = b.TimesExtended,
                            ExtensionsRemaining = 3 - b.TimesExtended,
                            CanExtend = b.TimesExtended < 3
                        })
                        .ToListAsync();
#pragma warning restore CS8602
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostExtendAsync(int borrowId)
        {
            var user = await GetCurrentUserAsync();
            if (user?.ReaderId == null)
            {
                return Challenge();
            }

            var borrow = await _context.Borrows
                .FirstOrDefaultAsync(b => b.Id == borrowId && b.ReaderId == user.ReaderId && b.ReturnDate == null);

            if (borrow == null)
            {
                return NotFound();
            }

            if (borrow.TimesExtended >= 3)
            {
                ModelState.AddModelError(string.Empty, "Cannot extend this borrow further. Maximum extensions reached.");
                return await OnGetAsync();
            }

            // Extend the return date by 14 days
            borrow.ExpectedReturnDate = borrow.ExpectedReturnDate.AddDays(14);
            borrow.TimesExtended++;

            _context.Update(borrow);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        private Task<ApplicationUser?> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
        }

        public class BorrowSummary
        {
            public int BorrowId { get; set; }
            public string BookTitle { get; set; } = string.Empty;
            public string Author { get; set; } = string.Empty;
            public int CopyId { get; set; }
            public int DeptId { get; set; }
            public DateTime BorrowDate { get; set; }
            public DateTime ExpectedReturnDate { get; set; }
            public int TimesExtended { get; set; }
            public int ExtensionsRemaining { get; set; }
            public bool CanExtend { get; set; }
        }
    }
}