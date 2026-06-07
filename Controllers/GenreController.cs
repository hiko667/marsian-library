using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using marsian_library.Models;
using marsian_library.Services;
using Microsoft.AspNetCore.Authorization;

namespace marsian_library.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: Genre
        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync();
            return View(genres);
        }

        // GET: Genre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null) return NotFound();

            return View(genre);
        }

        // GET: Genre/Create
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genre/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,ChildrenFriendly")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _genreService.CreateAsync(genre);
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genre/Edit/5
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null) return NotFound();
            
            return View(genre);
        }

        // POST: Genre/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ChildrenFriendly")] Genre genre)
        {
            if (id != genre.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _genreService.UpdateAsync(id, genre);
                if (!result) return NotFound();

                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genre/Delete/5
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null) return NotFound();

            return View(genre);
        }

        // POST: Genre/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Employee,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _genreService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}