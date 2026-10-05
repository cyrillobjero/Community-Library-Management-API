using Community_Library_Management_API.DTO.Books;

namespace Community_Library_Management_API.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();

        Task<BookDto?> GetByIdAsync(int id);

        Task<BookDto> CreateAsync(CreateBookDto dto);

        Task<BookDto?> UpdateAsync(int id, CreateBookDto dto);

        Task<bool> DeleteAsync(int id);
    }
}