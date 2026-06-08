using System.Collections.Generic;
using System.Threading.Tasks;
using marsian_library.Models;

namespace marsian_library.Services
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetAllAsync();
        Task<Publisher?> GetByIdAsync(int id);
        Task CreateAsync(Publisher publisher);
        Task UpdateAsync(Publisher publisher);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}