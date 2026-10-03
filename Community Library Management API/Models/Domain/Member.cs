namespace Community_Library_Management_API.Models.Domain
{
    public class Member
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MembershipType { get; set; } = string.Empty;

        public DateTime DateJoined { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public List<Loan> Loans { get; set; } = new();
    }
}
