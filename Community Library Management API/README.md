# Community Library Management API

## Project Description

The **Community Library Management API** is a RESTful Web API developed using **.NET 9, C#, Entity Framework Core, and SQL Server**.

The API manages three related resources:

* **Books** — stores information about library books.
* **Members** — stores registered library members.
* **Loans** — connects books and members and manages borrowing and returning.

The project demonstrates RESTful API design, Entity Framework Core database integration, CRUD operations, DTOs, repository and service layers, dependency injection, and business rules for borrowing and returning books.

## Setup and Run Instructions

### 1. Configure the Database Connection

Open `appsettings.json` and update the `DefaultConnection` to match your local SQL Server instance.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=CommunityLibraryDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance.

For example, depending on your SQL Server installation, it may look like:

```text
Server=localhost;Database=CommunityLibraryDb;Trusted_Connection=True;TrustServerCertificate=True;
```

Do not commit real passwords or other sensitive credentials to GitHub.

Yes — if you **did not use SSMS** and instead included the SQL scripts directly in your VS Code project, then the README should describe it that way.

You can replace that section with:

### 2. Create the Database

The project contains two SQL scripts inside the `database` folder:

```text
database/
├── database-design.sql
└── database-content.sql
```

The database setup is included directly in the project through these SQL scripts.

Run the scripts in the following order:

1. `database-design.sql`
2. `database-content.sql`

The `database-design.sql` script creates the `CommunityLibraryDb` database and its tables.

The database contains the following related tables:

* `Books`
* `Members`
* `Loans`

The `Loans` table is related to `Books` through `BookId` and to `Members` through `MemberId`.

The `database-content.sql` script inserts sample books, members, and loan records for testing the API.

### Important

The SQL files are **part of the project**, so the database structure and sample data can be recreated from the files without manually creating each table.

**Do not say "Open SQL Server Management Studio (SSMS)"** if your actual project workflow was done directly through VS Code.


### 3. Restore Dependencies

From the project directory, run:

```bash
dotnet restore
```

### 4. Build the Project

Run:

```bash
dotnet build
```

Make sure the project builds successfully before running the API.

### 5. Run the Application

From the project directory, run:

```bash
dotnet run
```

The API will display its running URL in the terminal.

Open the displayed URL in your browser.

### 6. Use Swagger

The project provides an interactive **Swagger/OpenAPI** interface for testing the API endpoints.

Open the Swagger URL provided by the application when it starts.

Swagger can be used to test:

* Books
* Members
* Loans
* Borrowing
* Returning books
* Member loan history

## API Endpoints

### Books

| Method | Endpoint          | Description       |
| ------ | ----------------- | ----------------- |
| GET    | `/api/books`      | Get all books     |
| GET    | `/api/books/{id}` | Get a book by ID  |
| POST   | `/api/books`      | Create a new book |
| PUT    | `/api/books/{id}` | Update a book     |
| DELETE | `/api/books/{id}` | Delete a book     |

### Members

| Method | Endpoint                  | Description                   |
| ------ | ------------------------- | ----------------------------- |
| GET    | `/api/members`            | Get all members               |
| GET    | `/api/members/{id}`       | Get a member by ID            |
| POST   | `/api/members`            | Register a new member         |
| PUT    | `/api/members/{id}`       | Update a member               |
| DELETE | `/api/members/{id}`       | Delete or deactivate a member |
| GET    | `/api/members/{id}/loans` | Get a member's loans          |

### Loans

| Method | Endpoint                 | Description            |
| ------ | ------------------------ | ---------------------- |
| GET    | `/api/loans`             | Get all loans          |
| GET    | `/api/loans/{id}`        | Get a loan by ID       |
| POST   | `/api/loans`             | Borrow a book          |
| POST   | `/api/loans/{id}/return` | Return a borrowed book |

## Loan Business Rules

The Loans resource contains the main business rules of the system.

### 1. Book Availability

A book can only be borrowed when copies are available.

When a book is borrowed:

```text
AvailableCopies - 1
```

When a book is returned:

```text
AvailableCopies + 1
```

Both changes are saved through Entity Framework Core.

### 2. Active Members

An inactive member cannot borrow a book.

### 3. Maximum Active Loans

A member can have a maximum of **3 active loans** at one time.

### 4. Due Date

The due date is automatically calculated as:

```text
BorrowedDate + 7 days
```

### 5. Not Found

Borrowing with a non-existent book or member returns:

```text
404 Not Found
```

### 6. Rule Violations

The following situations return:

```text
409 Conflict
```

* No available copies
* Inactive member
* Member already has 3 active loans

### 7. Returning a Loan

A loan that has already been returned cannot be returned again.

## Layer Structure and Dependency Injection

The project follows a **layered architecture**:

```text
Controllers
     ↓
Services
     ↓
Repositories
     ↓
AppDbContext
     ↓
SQL Server
```

### Controllers

Controllers handle HTTP requests and responses.

They receive requests from the client and call the appropriate service. Controllers do not directly access the database or contain the main business rules.

The project contains:

```text
Controllers/
├── BooksController.cs
├── MembersController.cs
└── LoansController.cs
```

### Services

Services contain the application's business logic.

They handle:

* Validation
* Book operations
* Member operations
* Borrowing rules
* Returning rules
* Loan limits
* Book availability

The project contains:

```text
Services/
├── Interfaces/
│   ├── IBookService.cs
│   ├── IMemberService.cs
│   └── ILoanService.cs
│
├── BookService.cs
├── MemberService.cs
└── LoanService.cs
```

### Repositories

Repositories handle database operations using Entity Framework Core.

The `AppDbContext` is used inside the repository layer rather than directly inside controllers or services.

The project contains:

```text
Repositories/
├── Interfaces/
│   ├── IBookRepository.cs
│   ├── IMemberRepository.cs
│   └── ILoanRepository.cs
│
├── BookRepository.cs
├── MemberRepository.cs
└── LoanRepository.cs
```

### DTOs

DTOs are used at the API boundary so that database entities are not directly exposed to clients.

The project contains DTOs for:

* Books
* Members
* Loans

```text
DTO/
├── Books/
├── Members/
└── Loans/
```

### Entity Framework Core

`AppDbContext` contains the database sets:

```text
Books
Members
Loans
```

Entity Framework Core is responsible for communicating with SQL Server.

Read-only operations use `AsNoTracking()`, while changes use EF Core tracking and `SaveChangesAsync()`.

### Dependency Injection

Dependency injection connects the application's layers.

The following components are registered using the **Scoped** lifetime:

* `AppDbContext`
* Repositories
* Services

Scoped lifetime is appropriate because the repositories use the `DbContext`, which is also scoped to an HTTP request.

The application does not manually create repositories or services using `new`.

## Database Structure

The database contains three main tables.

### Books

Stores library book information.

Important fields include:

```text
Id
Title
Author
ISBN
TotalCopies
AvailableCopies
```

### Members

Stores registered members.

Important fields include:

```text
Id
FullName
Email
MembershipType
DateJoined
IsActive
```

### Loans

Connects a member with a book.

Important fields include:

```text
Id
BookId
MemberId
BorrowedDate
DueDate
ReturnedDate
Status
```

The relationships are:

```text
Book 1 ──────── * Loan
Member 1 ────── * Loan
```

A book can have many loan records, and a member can have many loan records.

Each loan belongs to exactly one book and one member.

## Project Structure

```text
CommunityLibrary/
│
├── Controllers/
│   ├── BooksController.cs
│   ├── MembersController.cs
│   └── LoansController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Models/
│   └── Domain/
│       ├── Book.cs
│       ├── Member.cs
│       └── Loan.cs
│
├── DTO/
│   ├── Books/
│   │   ├── BookDto.cs
│   │   └── CreateBookDto.cs
│   │
│   ├── Members/
│   │   ├── MemberDto.cs
│   │   └── CreateMemberDto.cs
│   │
│   └── Loans/
│       ├── LoanDto.cs
│       ├── CreateLoanDto.cs
│       └── ReturnLoanDto.cs
│
├── Repositories/
│   ├── Interfaces/
│   │   ├── IBookRepository.cs
│   │   ├── IMemberRepository.cs
│   │   └── ILoanRepository.cs
│   │
│   ├── BookRepository.cs
│   ├── MemberRepository.cs
│   └── LoanRepository.cs
│
├── Services/
│   ├── Interfaces/
│   │   ├── IBookService.cs
│   │   ├── IMemberService.cs
│   │   └── ILoanService.cs
│   │
│   ├── BookService.cs
│   ├── MemberService.cs
│   └── LoanService.cs
│
├── database/
│   ├── database-design.sql
│   └── database-content.sql
│
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── CommunityLibrary.csproj
├── README.md
└── .gitignore
```

## Team & Contributions

### Lhanz

Built the **Books** resource, including:

* `Book.cs`
* `BookDto.cs`
* `CreateBookDto.cs`
* `IBookRepository.cs`
* `BookRepository.cs`
* `IBookService.cs`
* `BookService.cs`
* `BooksController.cs`

Also contributed to the project documentation and integration testing.

### Kim

Built the **Members** resource, including:

* `Member.cs`
* `MemberDto.cs`
* `CreateMemberDto.cs`
* `IMemberRepository.cs`
* `MemberRepository.cs`
* `IMemberService.cs`
* `MemberService.cs`
* `MembersController.cs`

Also contributed to validation and integration testing.

### Melvin

Built the **Loans** resource and cross-resource functionality, including:

* `Loan.cs`
* `LoanDto.cs`
* `CreateLoanDto.cs`
* `ReturnLoanDto.cs`
* `ILoanRepository.cs`
* `LoanRepository.cs`
* `ILoanService.cs`
* `LoanService.cs`
* `LoansController.cs`

Also worked on:

* `AppDbContext.cs`
* `database-design.sql`
* `database-content.sql`
* Dependency injection setup
* Swagger/OpenAPI configuration
* Loan business rules
* Integration testing

All three team members contributed to the integration and testing of the final API.

## GitHub Collaboration

The project uses a single GitHub repository with separate branches for each team member.

Each member works on their own feature branch and commits their work using their own GitHub account.

Example:

```text
main
│
├── Althea-Loanscontroller.cs, LoanDto.cs, ReturnLoanDto.cs, ILoanRepository.cs, LoanRepository.cs, ILoanService.cs, LoanService.cs
├── Cyrill-MembersController.cs, database-content.sql, CreateBookDto.cs, CreateLoanDto.cs, CreateMemberDto.cs, MemberDto.cs, IMemberRepository.cs, MemberRepository.cs, IMemberService.cs,MemberService.cs
└── Jacob-AppDbContext.cs, IBookService.cs, CreateBookDto.cs
```

Completed branches are merged into the `main` branch through pull requests after the work has been reviewed and tested.

The `main` branch contains the integrated final version of the Community Library Management API.

## Technologies Used

* **C#**
* **.NET 9**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **REST API**
* **Swagger/OpenAPI**
* **Git**
* **GitHub**

## Project Goal

The goal of this project is to demonstrate the concepts covered in **Modules 1–5**, including:

1. .NET 9 Web API project setup
2. RESTful API design
3. Entity Framework Core database integration
4. CRUD operations using EF Core
5. Repository and service layered architecture
6. DTO-based API design
7. Dependency injection
8. Database relationships
9. Business-rule validation
10. API documentation using Swagger/OpenAPI
