using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Controllers;

public class DeptController : Controller
{
    private readonly AppDbContext _context;

    public DeptController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Dept
    public async Task<IActionResult> Index()
    {
        var depts = _context.Depts
            .Include(d => d.Address)
            .Include(d => d.Director)
            .ThenInclude(d => d.Job);
        return View(await depts.ToListAsync());
    }

    // GET: Dept/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var dept = await _context.Depts
            .Include(d => d.Address)
            .Include(d => d.Director)
            .ThenInclude(d => d.Job)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (dept == null) return NotFound();

        return View(dept);
    }

    // GET: Dept/Create
    public async Task<IActionResult> Create()
    {
        // Pobierz tylko pracowników ze stanowiskiem "Director"
        var directors = await _context.Emps
            .Include(e => e.Job)
            .Where(e => e.Job != null && e.Job.Name == "Director")
            .ToListAsync();
        
        ViewData["DirectorId"] = new SelectList(directors, "Id", "FullName");
        ViewBag.ExistingAddresses = new SelectList(_context.Addresses, "Id", "FullAddress");
        
        return View();
    }

    // POST: Dept/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Dept dept, Address address, bool useExistingAddress, int? existingAddressId)
    {
        if (ModelState.IsValid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Obsługa adresu
                if (useExistingAddress && existingAddressId.HasValue)
                {
                    // Użyj istniejącego adresu
                    dept.AddressId = existingAddressId.Value;
                }
                else
                {
                    // Utwórz nowy adres
                    if (string.IsNullOrEmpty(address.City) || string.IsNullOrEmpty(address.Street) ||
                        string.IsNullOrEmpty(address.Building) || string.IsNullOrEmpty(address.ZipCode))
                    {
                        ModelState.AddModelError("", "All address fields are required when creating a new address.");
                        await PrepareCreateView(dept.DirectorId);
                        return View(dept);
                    }
                    
                    _context.Addresses.Add(address);
                    await _context.SaveChangesAsync();
                    dept.AddressId = address.Id;
                }

                _context.Add(dept);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                TempData["Success"] = "Department created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error creating department: {ex.Message}");
            }
        }

        await PrepareCreateView(dept.DirectorId);
        return View(dept);
    }

    // GET: Dept/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var dept = await _context.Depts
            .Include(d => d.Address)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dept == null) return NotFound();

        // Pobierz tylko pracowników ze stanowiskiem "Director"
        var directors = await _context.Emps
            .Include(e => e.Job)
            .Where(e => e.Job != null && e.Job.Name == "Director")
            .ToListAsync();
        
        ViewData["DirectorId"] = new SelectList(directors, "Id", "FullName", dept.DirectorId);
        ViewBag.ExistingAddresses = new SelectList(_context.Addresses, "Id", "FullAddress", dept.AddressId);
        
        return View(dept);
    }

    // POST: Dept/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Dept dept, Address address, bool useExistingAddress, int? existingAddressId)
    {
        if (id != dept.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var deptToUpdate = await _context.Depts
                    .Include(d => d.Address)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (deptToUpdate == null) return NotFound();

                // Aktualizuj podstawowe informacje
                deptToUpdate.DirectorId = dept.DirectorId;

                // Obsługa adresu
                if (useExistingAddress && existingAddressId.HasValue)
                {
                    deptToUpdate.AddressId = existingAddressId.Value;
                }
                else
                {
                    // Utwórz nowy adres
                    if (string.IsNullOrEmpty(address.City) || string.IsNullOrEmpty(address.Street) ||
                        string.IsNullOrEmpty(address.Building) || string.IsNullOrEmpty(address.ZipCode))
                    {
                        ModelState.AddModelError("", "All address fields are required when creating a new address.");
                        await PrepareEditView(dept.Id, dept.DirectorId);
                        return View(dept);
                    }
                    
                    _context.Addresses.Add(address);
                    await _context.SaveChangesAsync();
                    deptToUpdate.AddressId = address.Id;
                }

                _context.Update(deptToUpdate);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Department updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeptExists(dept.Id)) return NotFound();
                throw;
            }
        }

        await PrepareEditView(dept.Id, dept.DirectorId);
        return View(dept);
    }

    // GET: Dept/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var dept = await _context.Depts
            .Include(d => d.Address)
            .Include(d => d.Director)
            .ThenInclude(d => d.Job)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (dept == null) return NotFound();

        return View(dept);
    }

    // POST: Dept/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dept = await _context.Depts.FindAsync(id);
        if (dept != null) _context.Depts.Remove(dept);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DeptExists(int id)
    {
        return _context.Depts.Any(e => e.Id == id);
    }

    // Pomocnicza metoda do przygotowania widoku Create
    private async Task PrepareCreateView(int? selectedDirectorId = null)
    {
        var directors = await _context.Emps
            .Include(e => e.Job)
            .Where(e => e.Job != null && e.Job.Name == "Director")
            .ToListAsync();
        
        ViewData["DirectorId"] = new SelectList(directors, "Id", "FullName", selectedDirectorId);
        ViewBag.ExistingAddresses = new SelectList(_context.Addresses, "Id", "FullAddress");
    }

    // Pomocnicza metoda do przygotowania widoku Edit
    private async Task PrepareEditView(int deptId, int? selectedDirectorId = null)
    {
        var directors = await _context.Emps
            .Include(e => e.Job)
            .Where(e => e.Job != null && e.Job.Name == "Director")
            .ToListAsync();
        
        ViewData["DirectorId"] = new SelectList(directors, "Id", "FullName", selectedDirectorId);
        ViewBag.ExistingAddresses = new SelectList(_context.Addresses, "Id", "FullAddress");
    }
}