using Community_Library_Management_API.Models.Domain;

namespace Community_Library_Management_API.Repositories.Interfaces
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllAsync();

        Task<Loan?> GetByIdAsync(int id);

        Task<Loan?> GetByIdForUpdateAsync(int id);

        Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId);

        Task<int> CountActiveLoansAsync(int memberId);

        Task AddAsync(Loan loan);

        void Update(Loan loan);

        Task SaveChangesAsync();
    }
}