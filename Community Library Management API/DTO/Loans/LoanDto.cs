namespace Community_Library_Management_API.DTOs.Loans
{
    public class LoanDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string BookTitle { get; set; } = string.Empty;

        public int MemberId { get; set; }

        public string MemberName { get; set; } = string.Empty;

        public DateTime BorrowedDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}