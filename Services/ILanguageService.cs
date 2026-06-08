using System.Collections.Generic;
using System.Threading.Tasks;
using marsian_library.Models;

namespace marsian_library.Services
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetAllAsync();
        Task<Language> GetByIdAsync(int? id);
        Task CreateAsync(Language language);
        Task UpdateAsync(Language language);
        Task DeleteAsync(int id);
        bool Exists(int id);
    }
}