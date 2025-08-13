-- Script to create Courses table for SIMS Database
-- Run this script in SQL Server Management Studio 20

USE [SIMS]
GO

-- Check and drop table if exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Courses]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[Courses]
    PRINT 'Old Courses table dropped'
END
GO

-- Create Courses table
CREATE TABLE [dbo].[Courses](
    [CourseID] [int] IDENTITY(1,1) NOT NULL,
    [CourseName] [nvarchar](100) NOT NULL,
    [CourseCode] [nvarchar](20) NOT NULL,
    [Credits] [int] NOT NULL DEFAULT (3),
    [Duration] [nvarchar](50) NOT NULL,
    [Description] [nvarchar](500) NOT NULL,
    [Department] [nvarchar](100) NOT NULL,
    [Instructor] [nvarchar](100) NOT NULL,
    [Semester] [nvarchar](20) NOT NULL,
    [AcademicYear] [nvarchar](10) NOT NULL,
    [MaxStudents] [int] NULL,
    [Fee] [decimal](10,2) NULL,
    [CreatedAt] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime2](7) NULL,
    [IsActive] [bit] NOT NULL DEFAULT (1),
    CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED ([CourseID] ASC)
)
GO

-- Create unique index for CourseCode
CREATE UNIQUE NONCLUSTERED INDEX [IX_Courses_CourseCode] ON [dbo].[Courses]
(
    [CourseCode] ASC
) WHERE ([IsActive] = 1)
GO

-- Create index for CourseName for fast search
CREATE NONCLUSTERED INDEX [IX_Courses_CourseName] ON [dbo].[Courses]
(
    [CourseName] ASC
) WHERE ([IsActive] = 1)
GO

-- Create index for Department for filtering
CREATE NONCLUSTERED INDEX [IX_Courses_Department] ON [dbo].[Courses]
(
    [Department] ASC
) WHERE ([IsActive] = 1)
GO

-- Create index for Semester and AcademicYear for filtering
CREATE NONCLUSTERED INDEX [IX_Courses_Semester_AcademicYear] ON [dbo].[Courses]
(
    [Semester] ASC,
    [AcademicYear] ASC
) WHERE ([IsActive] = 1)
GO

-- Add sample data
INSERT INTO [dbo].[Courses] ([CourseName], [CourseCode], [Credits], [Duration], [Description], [Department], [Instructor], [Semester], [AcademicYear], [MaxStudents], [Fee])
VALUES 
    (N'Introduction to Computer Science', 'CS101', 3, N'16 weeks', N'Basic concepts of computer science and programming fundamentals', N'Computer Science', N'Dr. John Smith', N'Fall', N'2024-2025', 30, 1500.00),
    (N'Calculus I', 'MATH201', 4, N'16 weeks', N'Differential calculus with applications to science and engineering', N'Mathematics', N'Dr. Sarah Johnson', N'Fall', N'2024-2025', 25, 1800.00),
    (N'Physics for Engineers', 'PHY301', 4, N'16 weeks', N'Classical mechanics and thermodynamics for engineering students', N'Physics', N'Dr. Michael Brown', N'Fall', N'2024-2025', 35, 2000.00),
    (N'Organic Chemistry', 'CHEM401', 4, N'16 weeks', N'Structure, properties, and reactions of organic compounds', N'Chemistry', N'Dr. Emily Davis', N'Fall', N'2024-2025', 20, 2200.00),
    (N'Data Structures and Algorithms', 'CS301', 3, N'16 weeks', N'Advanced programming concepts and algorithm analysis', N'Computer Science', N'Dr. Robert Wilson', N'Fall', N'2024-2025', 28, 1600.00),
    (N'Linear Algebra', 'MATH301', 3, N'16 weeks', N'Vector spaces, linear transformations, and matrices', N'Mathematics', N'Dr. Lisa Garcia', N'Fall', N'2024-2025', 22, 1700.00),
    (N'Business Management', 'BUS101', 3, N'16 weeks', N'Principles of business management and organizational behavior', N'Business', N'Dr. Thomas Rodriguez', N'Fall', N'2024-2025', 40, 1400.00),
    (N'Microeconomics', 'ECON201', 3, N'16 weeks', N'Individual economic behavior and market structures', N'Economics', N'Dr. Maria Martinez', N'Fall', N'2024-2025', 35, 1500.00),
    (N'World Literature', 'LIT101', 3, N'16 weeks', N'Survey of world literature from ancient to modern times', N'Literature', N'Dr. James Anderson', N'Fall', N'2024-2025', 30, 1200.00),
    (N'Modern History', 'HIST201', 3, N'16 weeks', N'World history from the 18th century to present', N'History', N'Dr. Patricia Taylor', N'Fall', N'2024-2025', 25, 1300.00)
GO

-- Display table structure information
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Courses'
ORDER BY ORDINAL_POSITION
GO

-- Display sample data
SELECT * FROM [dbo].[Courses] ORDER BY [CourseID]
GO

PRINT 'Courses table created successfully with sample data!'
GO
