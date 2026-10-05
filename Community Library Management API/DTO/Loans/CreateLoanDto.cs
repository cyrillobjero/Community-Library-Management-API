namespace Community_Library_Management_API.DTOs.Loans
{
    public class CreateLoanDto
    {
        public int BookId { get; set; }

        public int MemberId { get; set; }
    }
}
