using marsian_library.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace marsian_library.Services;

public interface IDeptService
{
    Task<IEnumerable<Dept>> GetAllDeptsAsync();
    Task<Dept?> GetDeptByIdAsync(int id);
    Task<IEnumerable<Emp>> GetDirectorsAsync();
    Task<IEnumerable<Address>> GetAddressesAsync();
    Task<bool> CreateDeptAsync(Dept dept, Address address, bool useExistingAddress, int? existingAddressId);
    Task<bool> UpdateDeptAsync(int id, Dept dept, Address address, bool useExistingAddress, int? existingAddressId);
    Task<bool> DeleteDeptAsync(int id);
    Task<bool> DeptExistsAsync(int id);
}