-- Script to add TeacherID column to Students table
USE SIMS_DB;
GO

-- Add TeacherID column to Students table
ALTER TABLE Students
ADD TeacherID INT NULL;
GO

-- Add foreign key constraint
ALTER TABLE Students
ADD CONSTRAINT FK_Students_Teachers
FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID);
GO

PRINT 'TeacherID column added to Students table with foreign key constraint to Teachers table';
GO