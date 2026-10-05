using Community_Library_Management_API.Data;
using Community_Library_Management_API.Models.Domain;
using Community_Library_Management_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Community_Library_Management_API.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly AppDbContext _context;

        public LoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _context.Loans
                .AsNoTracking()
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToListAsync();
        }

        public async Task<Loan?> GetByIdAsync(int id)
        {
            return await _context.Loans
                .AsNoTracking()
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Loan?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Loans
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Loan>> GetByMemberIdAsync(
            int memberId)
        {
            return await _context.Loans
                .AsNoTracking()
                .Include(l => l.Book)
                .Include(l => l.Member)
                .Where(l => l.MemberId == memberId)
                .ToListAsync();
        }

        public async Task<int> CountActiveLoansAsync(int memberId)
        {
            return await _context.Loans
                .Where(l =>
                    l.MemberId == memberId &&
                    l.Status != "Returned")
                .CountAsync();
        }

        public async Task AddAsync(Loan loan)
        {
            await _context.Loans.AddAsync(loan);
        }

        public void Update(Loan loan)
        {
            _context.Loans.Update(loan);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}