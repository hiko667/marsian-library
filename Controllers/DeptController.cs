using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using marsian_library.Models;
using marsian_library.Services;

namespace marsian_library.Controllers;

public class DeptController : Controller
{
    private readonly IDeptService _deptService;

    public DeptController(IDeptService deptService)
    {
        _deptService = deptService;
    }

    // GET: Dept
    public async Task<IActionResult> Index()
    {
        var depts = await _deptService.GetAllDeptsAsync();
        return View(depts);
    }

    // GET: Dept/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var dept = await _deptService.GetDeptByIdAsync(id.Value);
        if (dept == null) return NotFound();

        return View(dept);
    }

    [Authorize(Roles = "Admin")]
    // GET: Dept/Create
    public async Task<IActionResult> Create()
    {
        await PrepareCreateView();
        return View();
    }

    // POST: Dept/Create
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Dept dept, Address address, bool useExistingAddress, int? existingAddressId)
    {
        if (ModelState.IsValid)
        {
            var success = await _deptService.CreateDeptAsync(dept, address, useExistingAddress, existingAddressId);
            if (success)
            {
                TempData["Success"] = "Department created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error creating department. Check address fields.");
        }

        await PrepareCreateView(dept.DirectorId);
        return View(dept);
    }

    // GET: Dept/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var dept = await _deptService.GetDeptByIdAsync(id.Value);
        if (dept == null) return NotFound();

        await PrepareEditView(dept.DirectorId, dept.AddressId);
        return View(dept);
    }

    // POST: Dept/Edit/5
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Dept dept, Address address, bool useExistingAddress, int? existingAddressId)
    {
        if (id != dept.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var success = await _deptService.UpdateDeptAsync(id, dept, address, useExistingAddress, existingAddressId);
            if (success)
            {
                TempData["Success"] = "Department updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Error updating department. Check address fields.");
        }

        await PrepareEditView(dept.DirectorId, dept.AddressId);
        return View(dept);
    }

    // GET: Dept/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var dept = await _deptService.GetDeptByIdAsync(id.Value);
        if (dept == null) return NotFound();

        return View(dept);
    }

    // POST: Dept/Delete/5
    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _deptService.DeleteDeptAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PrepareCreateView(int? selectedDirectorId = null)
    {
        var directors = await _deptService.GetDirectorsAsync();
        var addresses = await _deptService.GetAddressesAsync();

        ViewData["DirectorId"] = new SelectList(directors, "Id", "FullName", selectedDirectorId);
        ViewBag.ExistingAddresses = new SelectList(addresses, "Id", "FullAddress");
    }

    private async Task PrepareEditView(int? selectedDirectorId = null, int? selectedAddressId = null)
    {
        var directors = await _deptService.GetDirectorsAsync();
        var addresses = await _deptService.GetAddressesAsync();

        ViewData["DirectorId"] = new SelectList(directors, "Id", "FullName", selectedDirectorId);
        ViewBag.ExistingAddresses = new SelectList(addresses, "Id", "FullAddress", selectedAddressId);
    }
}