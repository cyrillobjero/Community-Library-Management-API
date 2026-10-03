using Community_Library_Management_API.Models.Domain;

namespace Community_Library_Management_API.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAllAsync();

        Task<Member?> GetByIdAsync(int id);

        Task AddAsync(Member member);

        void Update(Member member);

        void Delete(Member member);

        Task<bool> EmailExistsAsync(string email);

        Task SaveChangesAsync();
    }
}
