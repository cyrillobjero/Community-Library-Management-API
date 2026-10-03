USE CommunityLibraryDb;
GO

-- ============================================
-- BOOKS
-- ============================================

SET IDENTITY_INSERT Books ON;
GO

INSERT INTO Books
(
    Id,
    Title,
    Author,
    ISBN,
    Category,
    TotalCopies,
    AvailableCopies
)
VALUES
(
    1,
    'Noli Me Tangere',
    'Jose Rizal',
    '9780143039693',
    'Filipino Literature',
    5,
    4
),
(
    2,
    'The Great Gatsby',
    'F. Scott Fitzgerald',
    '9780743273565',
    'Classic',
    3,
    3
),
(
    3,
    'Clean Code',
    'Robert C. Martin',
    '9780132350884',
    'Programming',
    4,
    4
);
GO

SET IDENTITY_INSERT Books OFF;
GO


-- ============================================
-- MEMBERS
-- ============================================

SET IDENTITY_INSERT Members ON;
GO

INSERT INTO Members
(
    Id,
    FullName,
    Email,
    MembershipType,
    DateJoined,
    IsActive
)
VALUES
(
    1,
    'Juan Dela Cruz',
    'juan.delacruz@email.com',
    'Student',
    '2026-09-01',
    1
),
(
    2,
    'Maria Santos',
    'maria.santos@email.com',
    'Faculty',
    '2026-09-02',
    1
),
(
    3,
    'Pedro Reyes',
    'pedro.reyes@email.com',
    'Student',
    '2026-09-03',
    0
);
GO

SET IDENTITY_INSERT Members OFF;
GO


-- ============================================
-- LOANS
-- ============================================

SET IDENTITY_INSERT Loans ON;
GO

INSERT INTO Loans
(
    Id,
    BookId,
    MemberId,
    BorrowedDate,
    DueDate,
    ReturnedDate,
    Status
)
VALUES
(
    1,
    1,
    1,
    '2026-10-01 09:00:00',
    '2026-10-08 09:00:00',
    NULL,
    'Borrowed'
),
(
    2,
    2,
    2,
    '2026-09-20 10:00:00',
    '2026-09-27 10:00:00',
    '2026-09-25 14:00:00',
    'Returned'
);
GO

SET IDENTITY_INSERT Loans OFF;
GO