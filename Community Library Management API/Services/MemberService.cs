using Community_Library_Management_API.DTO.Members;
using Community_Library_Management_API.Models.Domain;
using Community_Library_Management_API.Repositories.Interfaces;
using Community_Library_Management_API.Services.Interfaces;

namespace Community_Library_Management_API.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MemberDto>> GetAllAsync()
        {
            var members = await _repository.GetAllAsync();

            return members.Select(MapToDto);
        }

        public async Task<MemberDto?> GetByIdAsync(int id)
        {
            var member = await _repository.GetByIdAsync(id);

            return member == null ? null : MapToDto(member);
        }

        public async Task<MemberDto> CreateAsync(CreateMemberDto dto)
        {
            ValidateMember(dto);

            var email = dto.Email.Trim();

            if (await _repository.EmailExistsAsync(email))
            {
                throw new ArgumentException(
                    "A member with this email already exists.");
            }

            var member = new Member
            {
                FullName = dto.FullName.Trim(),
                Email = email,
                MembershipType = dto.MembershipType.Trim(),
                DateJoined = DateTime.UtcNow,
                IsActive = true
            };

            await _repository.AddAsync(member);
            await _repository.SaveChangesAsync();

            return MapToDto(member);
        }

        public async Task<MemberDto?> UpdateAsync(
            int id,
            CreateMemberDto dto)
        {
            ValidateMember(dto);

            var member = await _repository.GetByIdAsync(id);

            if (member == null)
                return null;

            var email = dto.Email.Trim();

            if (!string.Equals(
                    member.Email,
                    email,
                    StringComparison.OrdinalIgnoreCase)
                && await _repository.EmailExistsAsync(email))
            {
                throw new ArgumentException(
                    "A member with this email already exists.");
            }

            member.FullName = dto.FullName.Trim();
            member.Email = email;
            member.MembershipType = dto.MembershipType.Trim();

            _repository.Update(member);

            await _repository.SaveChangesAsync();

            return MapToDto(member);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var member = await _repository.GetByIdAsync(id);

            if (member == null)
                return false;

            // Deactivate instead of physically deleting.
            member.IsActive = false;

            _repository.Update(member);

            await _repository.SaveChangesAsync();

            return true;
        }

        private static void ValidateMember(CreateMemberDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Full name is required.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.MembershipType))
                throw new ArgumentException(
                    "Membership type is required.");

            var type = dto.MembershipType.Trim();

            if (!type.Equals("Student", StringComparison.OrdinalIgnoreCase)
                && !type.Equals("Faculty", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Membership type must be Student or Faculty.");
            }
        }

        private static MemberDto MapToDto(Member member)
        {
            return new MemberDto
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                MembershipType = member.MembershipType,
                DateJoined = member.DateJoined,
                IsActive = member.IsActive
            };
        }
    }
}
