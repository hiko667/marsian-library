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
    public class BorrowHistoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public BorrowHistoryModel(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public bool IsReader { get; set; }
        public IList<BorrowItem> History { get; set; } = new List<BorrowItem>();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            if (user.ReaderId == null)
            {
                IsReader = false;
                return Page();
            }

            IsReader = true;

            History = await _context.Borrows
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Dept)
                .Where(b => b.ReaderId == user.ReaderId)
                .OrderByDescending(b => b.BorrowDate)
                .Select(b => new BorrowItem
                {
                    BookTitle = b.Copy!.Book!.Title,
                    CopyId = b.CopyId,
                    DeptId = b.Copy.Dept!.Id.ToString(),
                    BorrowDate = b.BorrowDate,
                    ExpectedReturnDate = b.ExpectedReturnDate,
                    ReturnDate = b.ReturnDate
                })
                .ToListAsync();

            return Page();
        }

        public class BorrowItem
        {
            public string BookTitle { get; set; } = string.Empty;
            public int CopyId { get; set; }
            public string DeptId { get; set; } = string.Empty;
            public DateTime BorrowDate { get; set; }
            public DateTime ExpectedReturnDate { get; set; }
            public DateTime? ReturnDate { get; set; }
        }
    }
}
