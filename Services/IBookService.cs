using System.Collections.Generic;
using System.Threading.Tasks;

namespace marsian_library.Services;

public interface IBookService
{
    Task<IEnumerable<object>> GetAllBooksAsync();
    Task<object?> GetBookByGuidAsync(string guid);
    Task<IEnumerable<object>> GetAvailableBooksAsync();

    Task<byte[]> GenerateExcelForUserBorrowsAsync(int userId);
}