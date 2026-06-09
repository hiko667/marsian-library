using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmpController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmpController(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Emp
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Emps
                .Include(e => e.Address)
                .Include(e => e.Dept)
                .Include(e => e.Job)
                .Include(e => e.ApplicationUser); 
            return View(await appDbContext.ToListAsync());
        }

        // GET: Emp/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emp = await _context.Emps
                .Include(e => e.Address)
                .Include(e => e.Dept)
                .Include(e => e.Job)
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emp == null)
            {
                return NotFound();
            }

            return View(emp);
        }

        // GET: Emp/Create
        public IActionResult Create()
        {
            PopulateSelectLists();
            return View(new EmpViewModel());
        }

        // POST: Emp/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateSelectLists(model);
                return View(model);
            }

            var emp = new Emp
            {
                AddressId = model.AddressId,
                DeptId = model.DeptId,
                JobId = model.JobId,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            _context.Add(emp);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError(nameof(model.Password), "Hasło jest wymagane, jeśli chcesz dodać konto.");
                    _context.Emps.Remove(emp);
                    await _context.SaveChangesAsync();
                    PopulateSelectLists(model);
                    return View(model);
                }

                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Podany adres e-mail jest już używany.");
                    _context.Emps.Remove(emp);
                    await _context.SaveChangesAsync();
                    PopulateSelectLists(model);
                    return View(model);
                }

                var newUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmpId = emp.Id,
                    EmailConfirmed = true
                };

                var createUserResult = await _userManager.CreateAsync(newUser, model.Password!);
                if (!createUserResult.Succeeded)
                {
                    _context.Emps.Remove(emp);
                    await _context.SaveChangesAsync();
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    PopulateSelectLists(model);
                    return View(model);
                }

                if (await _roleManager.RoleExistsAsync("Employee"))
                {
                    await _userManager.AddToRoleAsync(newUser, "Employee");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Emp/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emp = await _context.Emps.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.EmpId == emp.Id);

            var model = new EmpViewModel
            {
                Id = emp.Id,
                AddressId = emp.AddressId,
                DeptId = emp.DeptId,
                JobId = emp.JobId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = user?.Email,
                CurrentEmail = user?.Email
            };

            PopulateSelectLists(model);
            return View(model);
        }

        // POST: Emp/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmpViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                PopulateSelectLists(model);
                return View(model);
            }

            var emp = await _context.Emps.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

            emp.AddressId = model.AddressId;
            emp.DeptId = model.DeptId;
            emp.JobId = model.JobId;
            emp.FirstName = model.FirstName;
            emp.LastName = model.LastName;

            try
            {
                _context.Update(emp);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpExists(emp.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.EmpId == emp.Id);
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(model.Email) && !string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var otherUser = await _userManager.FindByEmailAsync(model.Email);
                    if (otherUser != null && otherUser.Id != user.Id)
                    {
                        ModelState.AddModelError(nameof(model.Email), "Ten adres e-mail jest już używany przez innego użytkownika.");
                        PopulateSelectLists(model);
                        return View(model);
                    }

                    user.Email = model.Email;
                    user.UserName = model.Email;
                    var updateResult = await _userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        PopulateSelectLists(model);
                        return View(model);
                    }
                }

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    if (await _userManager.HasPasswordAsync(user))
                    {
                        var removePassResult = await _userManager.RemovePasswordAsync(user);
                        if (!removePassResult.Succeeded)
                        {
                            foreach (var error in removePassResult.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            PopulateSelectLists(model);
                            return View(model);
                        }
                    }

                    var addPassResult = await _userManager.AddPasswordAsync(user, model.Password);
                    if (!addPassResult.Succeeded)
                    {
                        foreach (var error in addPassResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        PopulateSelectLists(model);
                        return View(model);
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(model.Email))
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError(nameof(model.Password), "Hasło jest wymagane, jeśli chcesz dodać konto.");
                    PopulateSelectLists(model);
                    return View(model);
                }

                var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingUserByEmail != null)
                {
                    ModelState.AddModelError(nameof(model.Email), "Podany adres e-mail jest już używany.");
                    PopulateSelectLists(model);
                    return View(model);
                }

                var newUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmpId = emp.Id,
                    EmailConfirmed = true
                };

                var createUserResult = await _userManager.CreateAsync(newUser, model.Password!);
                if (!createUserResult.Succeeded)
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    PopulateSelectLists(model);
                    return View(model);
                }

                if (await _roleManager.RoleExistsAsync("Employee"))
                {
                    await _userManager.AddToRoleAsync(newUser, "Employee");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Emp/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emp = await _context.Emps
                .Include(e => e.Address)
                .Include(e => e.Dept)
                .Include(e => e.Job)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emp == null)
            {
                return NotFound();
            }

            return View(emp);
        }

        // POST: Emp/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emp = await _context.Emps.FindAsync(id);
            if (emp != null)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.EmpId == emp.Id);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                _context.Emps.Remove(emp);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmpExists(int id)
        {
            return _context.Emps.Any(e => e.Id == id);
        }

        private void PopulateSelectLists(EmpViewModel? model = null)
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Building", model?.AddressId);
            ViewData["DeptId"] = new SelectList(_context.Depts, "Id", "Id", model?.DeptId);
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Name", model?.JobId);
        }
    }
}
