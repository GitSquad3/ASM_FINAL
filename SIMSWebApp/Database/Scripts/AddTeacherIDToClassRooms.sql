-- Script to add TeacherID column to ClassRooms table
USE SIMS_DB;
GO

-- Add TeacherID column to ClassRooms table
ALTER TABLE ClassRooms
ADD TeacherID INT NULL;
GO

-- Add foreign key constraint
ALTER TABLE ClassRooms
ADD CONSTRAINT FK_ClassRooms_Teachers
FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID);
GO

-- Update SIMSDbContext configuration
PRINT 'TeacherID column added to ClassRooms table with foreign key constraint to Teachers table';
GO