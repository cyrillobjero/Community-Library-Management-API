namespace Community_Library_Management_API.DTO.Members
{
    public class MemberDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MembershipType { get; set; } = string.Empty;

        public DateTime DateJoined { get; set; }

        public bool IsActive { get; set; }
    }
}
