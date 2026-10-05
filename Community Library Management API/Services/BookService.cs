using Community_Library_Management_API.Models.Domain;
using Community_Library_Management_API.Repositories.Interfaces;
using CommunityLibrary.DTO.Books;
using CommunityLibrary.DTOs.Books;
using CommunityLibrary.Services.Interfaces;

namespace Community_Library_Management_API.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CommunityLibrary.DTOs.Books.BookDto>> GetAllAsync()
        {
            var books = await _repository.GetAllAsync();

            return (IEnumerable<CommunityLibrary.DTOs.Books.BookDto>)books.Select(MapToDto);
        }

        public async Task<CreateBookDto?> GetByIdAsync(int id)
        {
            var book = await _repository.GetByIdAsync(id);

            return book == null ? null : MapToDto(book);
        }

        public async Task<CreateBookDto> CreateAsync(CreateBookDto dto)
        {
            ValidateBook(dto);

            if (await _repository.ISBNExistsAsync(dto.ISBN))
            {
                throw new ArgumentException(
                    "A book with this ISBN already exists.");
            }

            var book = new Book
            {
                Title = dto.Title.Trim(),
                Author = dto.Author.Trim(),
                ISBN = dto.ISBN.Trim(),
                IsAvailable = true
            };

            await _repository.AddAsync(book);
            await _repository.SaveChangesAsync();

            return MapToDto(book);
        }

        public async Task<CreateBookDto?> UpdateAsync(
            int id, CreateBookDto dto)
        {
            ValidateBook(dto);

            var book = await _repository.GetByIdAsync(id);

            if (book == null)
                return null;

            if (!string.Equals(
                    book.ISBN, dto.ISBN.Trim(),
                    StringComparison.OrdinalIgnoreCase)
                && await _repository.ISBNExistsAsync(dto.ISBN.Trim()))
            {
                throw new ArgumentException(
                    "A book with this ISBN already exists.");
            }

            book.Title = dto.Title.Trim();
            book.Author = dto.Author.Trim();
            book.ISBN = dto.ISBN.Trim();

            _repository.Update(book);
            await _repository.SaveChangesAsync();

            return MapToDto(book);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _repository.GetByIdAsync(id);

            if (book == null)
                return false;

            _repository.Delete(book);
            await _repository.SaveChangesAsync();

            return true;
        }

        private static void ValidateBook(CreateBookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required.");

            if (string.IsNullOrWhiteSpace(dto.Author))
                throw new ArgumentException("Author is required.");

            if (string.IsNullOrWhiteSpace(dto.ISBN))
                throw new ArgumentException("ISBN is required.");
        }

        private static CreateBookDto MapToDto(Book book)
        {
            return new CreateBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                IsAvailable = book.IsAvailable
            };
        }

        Task<IEnumerable<BookDto>> IBookService.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<BookDto?> IBookService.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<BookDto> IBookService.CreateAsync(CreateBookDto dto)
        {
            throw new NotImplementedException();
        }

        Task<BookDto?> IBookService.UpdateAsync(int id, CreateBookDto dto)
        {
            throw new NotImplementedException();
        }

        Task<bool> IBookService.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
