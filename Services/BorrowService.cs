using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using marsian_library.Data;
using marsian_library.Models;

namespace marsian_library.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly AppDbContext _context;

        public BorrowService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Borrow>> GetAllBorrowsAsync()
        {
            return await _context.Borrows
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Include(b => b.Reader)
                .OrderByDescending(b => b.ReturnDate == null)
                .ThenByDescending(b => b.BorrowDate)
                .ToListAsync();
        }

        public async Task<Borrow?> GetBorrowByIdAsync(int id)
        {
            return await _context.Borrows
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .Include(b => b.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateBorrowAsync(Borrow borrow)
        {
            _context.Add(borrow);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBorrowAsync(Borrow borrow)
        {
            _context.Update(borrow);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBorrowAsync(int id)
        {
            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow != null)
            {
                _context.Borrows.Remove(borrow);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> BorrowExistsAsync(int id)
        {
            return await _context.Borrows.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> ReturnBookAsync(int id)
        {
            var borrow = await _context.Borrows
                .Include(b => b.Copy)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (borrow == null)
            {
                return false;
            }

            if (borrow.ReturnDate == null)
            {
                borrow.ReturnDate = DateTime.Now;
                var availableState = await _context.States.FirstOrDefaultAsync(s => s.Name == "Available");
                if (availableState == null)
                {
                    throw new InvalidOperationException("State 'Available' not found in database.");
                }

                if (borrow.Copy != null)
                {
                    borrow.Copy.StateId = availableState.Id;
                    _context.Copies.Update(borrow.Copy);
                }

                _context.Borrows.Update(borrow);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<Copy>> GetCopiesAsync()
        {
            return await _context.Copies.ToListAsync();
        }

        public async Task<IEnumerable<Reader>> GetReadersAsync()
        {
            return await _context.Readers.ToListAsync();
        }

        public async Task<string?> GetBookTitleByBorrowIdAsync(int borrowId)
        {
            var borrow = await _context.Borrows
                .Include(b => b.Copy)
                    .ThenInclude(c => c.Book)
                .FirstOrDefaultAsync(b => b.Id == borrowId);

            return borrow?.Copy?.Book?.Title;
        }
    }
}