using System.Collections.Generic;
using System.Threading.Tasks;
using marsian_library.Models;

namespace marsian_library.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int? id);
        Task CreateAsync(Genre genre);
        Task<bool> UpdateAsync(int id, Genre genre);
        Task DeleteAsync(int id);
        bool GenreExists(int id);
    }
}