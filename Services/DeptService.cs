using marsian_library.Data;
using marsian_library.Models;
using Microsoft.EntityFrameworkCore;

namespace marsian_library.Services;

public class DeptService : IDeptService
{
    private readonly AppDbContext _context;

    public DeptService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dept>> GetAllDeptsAsync()
    {
        return await _context.Depts
            .Include(d => d.Address)
            .Include(d => d.Director)
            .ThenInclude(d => d.Job)
            .ToListAsync();
    }

    public async Task<Dept?> GetDeptByIdAsync(int id)
    {
        return await _context.Depts
            .Include(d => d.Address)
            .Include(d => d.Director)
            .ThenInclude(d => d.Job)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Emp>> GetDirectorsAsync()
    {
        return await _context.Emps
            .Include(e => e.Job)
            .Where(e => e.Job != null && e.Job.Name == "Director")
            .ToListAsync();
    }

    public async Task<IEnumerable<Address>> GetAddressesAsync()
    {
        return await _context.Addresses.ToListAsync();
    }

    public async Task<bool> CreateDeptAsync(Dept dept, Address address, bool useExistingAddress, int? existingAddressId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (useExistingAddress && existingAddressId.HasValue)
            {
                dept.AddressId = existingAddressId.Value;
            }
            else
            {
                if (string.IsNullOrEmpty(address.City) || string.IsNullOrEmpty(address.Street) ||
                    string.IsNullOrEmpty(address.Building) || string.IsNullOrEmpty(address.ZipCode))
                {
                    return false;
                }

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                dept.AddressId = address.Id;
            }

            _context.Add(dept);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> UpdateDeptAsync(int id, Dept dept, Address address, bool useExistingAddress, int? existingAddressId)
    {
        try
        {
            var deptToUpdate = await _context.Depts
                .Include(d => d.Address)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (deptToUpdate == null) return false;

            deptToUpdate.DirectorId = dept.DirectorId;

            if (useExistingAddress && existingAddressId.HasValue)
            {
                deptToUpdate.AddressId = existingAddressId.Value;
            }
            else
            {
                if (string.IsNullOrEmpty(address.City) || string.IsNullOrEmpty(address.Street) ||
                    string.IsNullOrEmpty(address.Building) || string.IsNullOrEmpty(address.ZipCode))
                {
                    return false;
                }

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                deptToUpdate.AddressId = address.Id;
            }

            _context.Update(deptToUpdate);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await DeptExistsAsync(id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteDeptAsync(int id)
    {
        var dept = await _context.Depts.FindAsync(id);
        if (dept == null) return false;

        _context.Depts.Remove(dept);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeptExistsAsync(int id)
    {
        return await _context.Depts.AnyAsync(e => e.Id == id);
    }
}