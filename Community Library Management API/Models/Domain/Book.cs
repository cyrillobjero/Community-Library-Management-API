namespace Community_Library_Management_API.Models.Domain
{
    public class Book
    {
        internal int TotalCopies;

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public List<Loan> Loans { get; set; } = new();
<<<<<<< HEAD
        public int AvailableCopies { get; internal set; }
=======
        public bool IsAvailable { get; internal set; }
>>>>>>> master
    }
}