using Community_Library_Management_API.DTO.Loans;

namespace Community_Library_Management_API.Services.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanDto>> GetAllAsync();

        Task<LoanDto?> GetByIdAsync(int id);

        Task<LoanDto> CreateAsync(CreateLoanDto dto);

        Task<LoanDto?> ReturnAsync(int id);

        Task<IEnumerable<LoanDto>> GetByMemberIdAsync(int memberId);
    }
}