using Community_Library_Management_API.Models.Domain;

namespace Community_Library_Management_API.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();

        Task<Book?> GetByIdAsync(int id);

        Task AddAsync(Book book);

        void Update(Book book);

        void Delete(Book book);

        Task<bool> ISBNExistsAsync(string isbn);

        Task SaveChangesAsync();
    }
}
