using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using marsian_library.Models;
using marsian_library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace marsian_library.Controllers
{
    [Authorize(Roles = "Employee,Admin")]
    public class LanguageController : Controller
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        // GET: Language
        public async Task<IActionResult> Index()
        {
            var languages = await _languageService.GetAllAsync();
            return View(languages);
        }

        // GET: Language/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _languageService.GetByIdAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // GET: Language/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Language/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Language language)
        {
            if (ModelState.IsValid)
            {
                await _languageService.CreateAsync(language);
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // GET: Language/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _languageService.GetByIdAsync(id);
            if (language == null)
            {
                return NotFound();
            }
            return View(language);
        }

        // POST: Language/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Language language)
        {
            if (id != language.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _languageService.UpdateAsync(language);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_languageService.Exists(language.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // GET: Language/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var language = await _languageService.GetByIdAsync(id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // POST: Language/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _languageService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}