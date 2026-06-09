using Microsoft.AspNetCore.Mvc;
using marsian_library.Services;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using marsian_library.Models;
using Microsoft.AspNetCore.Identity;

namespace marsian_library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookApiController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BookApiController(IBookService bookService, UserManager<ApplicationUser> userManager)
    {
        _bookService = bookService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{guid}")]
    public async Task<IActionResult> GetBook(string guid)
    {
        var book = await _bookService.GetBookByGuidAsync(guid);
        
        if (book == null)
        {
            return NotFound(new { message = $"Nie znaleziono książki o identyfikatorze GUID: {guid}" });
        }

        return Ok(book);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    {
        var books = await _bookService.GetAvailableBooksAsync();
        return Ok(books);
    }

    [HttpGet("export")]
    [Authorize]
    public async Task<IActionResult> ExportMyBorrowsToExcel()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || !user.ReaderId.HasValue)
        {
            return NotFound("Zalogowane konto nie posiada przypisanego profilu czytelnika.");
        }

        byte[] excelBytes = await _bookService.GenerateExcelForUserBorrowsAsync(user.ReaderId.Value);
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        string fileName = $"wypozyczenia_{userId}_{DateTime.Now:yyyyMMdd}.xlsx";

        return File(excelBytes, contentType, fileName);
    }
}