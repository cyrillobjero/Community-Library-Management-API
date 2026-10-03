using CommunityLibrary.DTO.Books;
using CommunityLibrary.DTOs.Books;

namespace CommunityLibrary.Services.Interfaces
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