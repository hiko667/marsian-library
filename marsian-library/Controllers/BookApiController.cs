using Microsoft.AspNetCore.Mvc;
using marsian_library.Services;
using System.Threading.Tasks;

namespace marsian_library.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookApiController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookApiController(IBookService bookService)
    {
        _bookService = bookService;
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
}