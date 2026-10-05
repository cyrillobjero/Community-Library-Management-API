namespace Community_Library_Management_API.DTO.Books
{
<<<<<<< HEAD
    public class BookDto    
    {
        public int Id { get; set; }
=======
    public class BookDto
    {
        public int Id { get; set; }

>>>>>>> master
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;
<<<<<<< HEAD
        public bool IsAvailable { get; set; }
=======

        public string Category { get; set; } = string.Empty;

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }
        public bool IsAvailable { get; internal set; }
>>>>>>> master
    }
}