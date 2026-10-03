using Community_Library_Management_API.DTO.Members;

namespace Community_Library_Management_API.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetAllAsync();

        Task<MemberDto?> GetByIdAsync(int id);

        Task<MemberDto> CreateAsync(CreateMemberDto dto);

        Task<MemberDto?> UpdateAsync(int id, CreateMemberDto dto);

        Task<bool> DeleteAsync(int id);
    }
}