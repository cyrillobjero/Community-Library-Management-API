namespace Community_Library_Management_API.DTO.Members
{
    public class CreateMemberDto
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string MembershipType { get; set; } = string.Empty;
    }
}
