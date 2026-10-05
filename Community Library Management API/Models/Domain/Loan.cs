namespace Community_Library_Management_API.Models.Domain
{
    public class Loan
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int MemberId { get; set; }

        public DateTime BorrowedDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public string Status { get; set; } = "Borrowed";

        public Book Book { get; set; } = null!;

        public Member Member { get; set; } = null!;
    }
}
