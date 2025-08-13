-- Script to create Students table for SIMS Database
-- Run this script in SQL Server Management Studio 20

USE [SIMS]
GO

-- Check and drop table if exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Students]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[Students]
    PRINT 'Old Students table dropped'
END
GO

-- Create Students table
CREATE TABLE [dbo].[Students](
    [StudentID] [int] IDENTITY(1,1) NOT NULL,
    [FullName] [nvarchar](100) NOT NULL,
    [StudentCode] [nvarchar](20) NOT NULL,
    [DateOfBirth] [date] NOT NULL,
    [Gender] [nvarchar](10) NOT NULL,
    [Address] [nvarchar](200) NOT NULL,
    [PhoneNumber] [nvarchar](15) NOT NULL,
    [Email] [nvarchar](100) NULL,
    [Class] [nvarchar](20) NOT NULL,
    [CreatedAt] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime2](7) NULL,
    [IsActive] [bit] NOT NULL DEFAULT (1),
    CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED ([StudentID] ASC)
)
GO

-- Create unique index for StudentCode
CREATE UNIQUE NONCLUSTERED INDEX [IX_Students_StudentCode] ON [dbo].[Students]
(
    [StudentCode] ASC
) WHERE ([IsActive] = 1)
GO

-- Create index for FullName for fast search
CREATE NONCLUSTERED INDEX [IX_Students_FullName] ON [dbo].[Students]
(
    [FullName] ASC
) WHERE ([IsActive] = 1)
GO

-- Create index for Class for filtering
CREATE NONCLUSTERED INDEX [IX_Students_Class] ON [dbo].[Students]
(
    [Class] ASC
) WHERE ([IsActive] = 1)
GO

-- Add sample data
INSERT INTO [dbo].[Students] ([FullName], [StudentCode], [DateOfBirth], [Gender], [Address], [PhoneNumber], [Email], [Class])
VALUES 
    (N'John Smith', 'SM2024001', '2006-05-15', N'Male', N'123 ABC Street, District 1, HCMC', '0901234567', 'john.smith@email.com', N'10A1'),
    (N'Jane Doe', 'DO2024002', '2006-08-22', N'Female', N'456 XYZ Street, District 2, HCMC', '0901234568', 'jane.doe@email.com', N'10A1'),
    (N'Mike Johnson', 'JO2024003', '2006-03-10', N'Male', N'789 DEF Street, District 3, HCMC', '0901234569', 'mike.johnson@email.com', N'10A2'),
    (N'Sarah Wilson', 'WI2024004', '2006-11-05', N'Female', N'321 GHI Street, District 4, HCMC', '0901234570', 'sarah.wilson@email.com', N'10A2'),
    (N'David Brown', 'BR2024005', '2006-07-18', N'Male', N'654 JKL Street, District 5, HCMC', '0901234571', 'david.brown@email.com', N'10A3'),
    (N'Emily Davis', 'DA2024006', '2006-09-30', N'Female', N'987 MNO Street, District 6, HCMC', '0901234572', 'emily.davis@email.com', N'10A3'),
    (N'Robert Miller', 'MI2024007', '2006-01-12', N'Male', N'147 PQR Street, District 7, HCMC', '0901234573', 'robert.miller@email.com', N'10A4'),
    (N'Lisa Garcia', 'GA2024008', '2006-04-25', N'Female', N'258 STU Street, District 8, HCMC', '0901234574', 'lisa.garcia@email.com', N'10A4'),
    (N'Thomas Rodriguez', 'RO2024009', '2006-06-08', N'Male', N'369 VWX Street, District 9, HCMC', '0901234575', 'thomas.rodriguez@email.com', N'10A5'),
    (N'Maria Martinez', 'MA2024010', '2006-12-14', N'Female', N'741 YZA Street, District 10, HCMC', '0901234576', 'maria.martinez@email.com', N'10A5')
GO

-- Display table structure information
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Students'
ORDER BY ORDINAL_POSITION
GO

-- Display sample data
SELECT * FROM [dbo].[Students] ORDER BY [StudentID]
GO

PRINT 'Students table created successfully with sample data!'
GO
