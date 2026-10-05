using Community_Library_Management_API.DTOs.Loans;
using Community_Library_Management_API.Models.Domain;
using Community_Library_Management_API.Repositories.Interfaces;
using Community_Library_Management_API.Services.Interfaces;

namespace Community_Library_Management_API.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;

        public LoanService(
            ILoanRepository loanRepository,
            IBookRepository bookRepository,
            IMemberRepository memberRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<LoanDto>> GetAllAsync()
        {
            var loans = await _loanRepository.GetAllAsync();

            return loans.Select(MapToDto);
        }

        public async Task<LoanDto?> GetByIdAsync(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);

            return loan == null ? null : MapToDto(loan);
        }

        public async Task<LoanDto> CreateAsync(CreateLoanDto dto)
        {
            if (dto.BookId <= 0)
                throw new ArgumentException("BookId is required.");

            if (dto.MemberId <= 0)
                throw new ArgumentException("MemberId is required.");

            var book = await _bookRepository.GetByIdAsync(dto.BookId);

            if (book == null)
            {
                throw new KeyNotFoundException(
                    "Book not found.");
            }

            var member = await _memberRepository.GetByIdAsync(
                dto.MemberId);

            if (member == null)
            {
                throw new KeyNotFoundException(
                    "Member not found.");
            }

            if (!member.IsActive)
            {
                throw new InvalidOperationException(
                    "Inactive members cannot borrow books.");
            }

            var activeLoans =
                await _loanRepository.CountActiveLoansAsync(
                    dto.MemberId);

            if (activeLoans >= 3)
            {
                throw new InvalidOperationException(
                    "A member may hold at most 3 active loans.");
            }

            if (book.AvailableCopies <= 0)
            {
                throw new InvalidOperationException(
                    "No available copies of this book.");
            }

            var borrowedDate = DateTime.UtcNow;

            var loan = new Loan
            {
                BookId = book.Id,
                MemberId = member.Id,
                BorrowedDate = borrowedDate,
                DueDate = borrowedDate.AddDays(7),
                ReturnedDate = null,
                Status = "Borrowed"
            };

            book.AvailableCopies--;

            _bookRepository.Update(book);

            await _loanRepository.AddAsync(loan);

            await _loanRepository.SaveChangesAsync();

            loan.Book = book;
            loan.Member = member;

            return MapToDto(loan);
        }

        public async Task<LoanDto?> ReturnAsync(int id)
        {
            var loan = await _loanRepository.GetByIdForUpdateAsync(id);

            if (loan == null)
            {
                throw new KeyNotFoundException(
                    "Loan not found.");
            }

            if (loan.Status == "Returned"
                || loan.ReturnedDate != null)
            {
                throw new InvalidOperationException(
                    "This loan has already been returned.");
            }

            var book = await _bookRepository.GetByIdAsync(
                loan.BookId);

            if (book == null)
            {
                throw new KeyNotFoundException(
                    "Book associated with this loan was not found.");
            }

            book.AvailableCopies++;

            if (book.AvailableCopies > book.TotalCopies)
            {
                book.AvailableCopies = book.TotalCopies;
            }

            loan.ReturnedDate = DateTime.UtcNow;
            loan.Status = "Returned";

            _bookRepository.Update(book);
            _loanRepository.Update(loan);

            await _loanRepository.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<IEnumerable<LoanDto>> GetByMemberIdAsync(
            int memberId)
        {
            var member = await _memberRepository.GetByIdAsync(
                memberId);

            if (member == null)
            {
                throw new KeyNotFoundException(
                    "Member not found.");
            }

            var loans =
                await _loanRepository.GetByMemberIdAsync(memberId);

            return loans.Select(MapToDto);
        }

        private static LoanDto MapToDto(Loan loan)
        {
            return new LoanDto
            {
                Id = loan.Id,

                BookId = loan.BookId,

                BookTitle = loan.Book?.Title ?? string.Empty,

                MemberId = loan.MemberId,

                MemberName = loan.Member?.FullName ?? string.Empty,

                BorrowedDate = loan.BorrowedDate,

                DueDate = loan.DueDate,

                ReturnedDate = loan.ReturnedDate,

                Status = loan.Status
            };
        }
    }
}