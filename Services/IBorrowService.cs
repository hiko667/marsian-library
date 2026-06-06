using System.Collections.Generic;
using System.Threading.Tasks;
using marsian_library.Models;

namespace marsian_library.Services
{
    public interface IBorrowService
    {
        Task<IEnumerable<Borrow>> GetAllBorrowsAsync();
        Task<Borrow?> GetBorrowByIdAsync(int id);
        Task CreateBorrowAsync(Borrow borrow);
        Task UpdateBorrowAsync(Borrow borrow);
        Task DeleteBorrowAsync(int id);
        Task<bool> BorrowExistsAsync(int id);
        Task<bool> ReturnBookAsync(int id);
        Task<IEnumerable<Copy>> GetCopiesAsync();
        Task<IEnumerable<Reader>> GetReadersAsync();
    }
}