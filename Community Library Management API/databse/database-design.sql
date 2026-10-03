IF DB_ID('CommunityLibraryDb') IS NULL
BEGIN
    CREATE DATABASE CommunityLibraryDb;
END
GO

USE CommunityLibraryDb;
GO


-- ============================================
-- BOOKS
-- ============================================

CREATE TABLE Books
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Title NVARCHAR(200) NOT NULL,

    Author NVARCHAR(150) NOT NULL,

    ISBN NVARCHAR(20) NOT NULL UNIQUE,

    Category NVARCHAR(100) NOT NULL,

    TotalCopies INT NOT NULL,

    AvailableCopies INT NOT NULL,

    CONSTRAINT CK_Books_TotalCopies
        CHECK (TotalCopies > 0),

    CONSTRAINT CK_Books_AvailableCopies
        CHECK (
            AvailableCopies >= 0
            AND AvailableCopies <= TotalCopies
        )
);
GO


-- ============================================
-- MEMBERS
-- ============================================

CREATE TABLE Members
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    FullName NVARCHAR(150) NOT NULL,

    Email NVARCHAR(255) NOT NULL UNIQUE,

    MembershipType NVARCHAR(20) NOT NULL,

    DateJoined DATETIME2 NOT NULL
        DEFAULT SYSDATETIME(),

    IsActive BIT NOT NULL
        DEFAULT 1,

    CONSTRAINT CK_Members_MembershipType
        CHECK (
            MembershipType IN ('Student', 'Faculty')
        )
);
GO


-- ============================================
-- LOANS
-- ============================================

CREATE TABLE Loans
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    BookId INT NOT NULL,

    MemberId INT NOT NULL,

    BorrowedDate DATETIME2 NOT NULL
        DEFAULT SYSDATETIME(),

    DueDate DATETIME2 NOT NULL,

    ReturnedDate DATETIME2 NULL,

    Status NVARCHAR(20) NOT NULL
        DEFAULT 'Borrowed',

    CONSTRAINT FK_Loans_Books
        FOREIGN KEY (BookId)
        REFERENCES Books(Id)
        ON DELETE NO ACTION,

    CONSTRAINT FK_Loans_Members
        FOREIGN KEY (MemberId)
        REFERENCES Members(Id)
        ON DELETE NO ACTION,

    CONSTRAINT CK_Loans_DueDate
        CHECK (DueDate >= BorrowedDate),

    CONSTRAINT CK_Loans_Status
        CHECK (
            Status IN ('Borrowed', 'Returned', 'Overdue')
        )
);
GO