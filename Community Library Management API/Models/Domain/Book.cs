using Community_Library_Management_API.Models.Domain;

namespace Community_Library_Management_API.Models.Domain
{
    public class Book
    {
        internal int TotalCopies;

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        public List<Loan> Loans { get; set; } = new();
        public int AvailableCopies { get; internal set; }
    }
}