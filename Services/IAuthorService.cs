using System.Collections.Generic;
using System.Threading.Tasks;
using marsian_library.Models;

namespace marsian_library.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorByIdAsync(int id);
        Task AddAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(int id);
        bool AuthorExists(int id);
    }
}