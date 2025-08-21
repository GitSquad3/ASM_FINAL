-- =============================================
-- SIMS Database Creation Script - Final Version
-- Student Information Management System
-- =============================================

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'SIMS_DB')
BEGIN
    ALTER DATABASE SIMS_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SIMS_DB;
END
GO

-- Create new database
CREATE DATABASE SIMS_DB;
GO

USE SIMS_DB;
GO

-- =============================================
-- Create Tables
-- =============================================

-- 1. Users Table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PhoneNumber NVARCHAR(15),
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Teacher', 'Student')),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2
);
GO

-- 2. Teachers Table
CREATE TABLE Teachers (
    TeacherID INT IDENTITY(1,1) PRIMARY KEY,
    TeacherCode NVARCHAR(20) UNIQUE NOT NULL,
    UserID INT UNIQUE NOT NULL,
    Department NVARCHAR(100) NOT NULL,
    Specialization NVARCHAR(100),
    AcademicDegree NVARCHAR(50),
    HireDate DATE NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
GO

-- 3. Students Table
CREATE TABLE Students (
    StudentID INT IDENTITY(1,1) PRIMARY KEY,
    StudentCode NVARCHAR(20) UNIQUE NOT NULL,
    UserID INT UNIQUE NOT NULL,
    Class NVARCHAR(20) NOT NULL,
    Major NVARCHAR(100) NOT NULL,
    EnrollmentDate DATE NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
GO

-- 4. Courses Table
CREATE TABLE Courses (
    CourseID INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode NVARCHAR(20) UNIQUE NOT NULL,
    CourseName NVARCHAR(100) NOT NULL,
    Credits INT NOT NULL,
    Duration NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500),
    Department NVARCHAR(100) NOT NULL,
    Semester NVARCHAR(20) NOT NULL,
    AcademicYear NVARCHAR(10) NOT NULL,
    MaxStudents INT DEFAULT 50,
    Fee DECIMAL(10,2) DEFAULT 0,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2
);
GO

-- 5. CourseAssignments Table
CREATE TABLE CourseAssignments (
    AssignmentID INT IDENTITY(1,1) PRIMARY KEY,
    CourseID INT NOT NULL,
    TeacherID INT NOT NULL,
    AssignmentDate DATETIME2 DEFAULT GETDATE(),
    Notes NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE,
    FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID) ON DELETE CASCADE,
UNIQUE(CourseID, TeacherID)
);
GO

-- 6. Enrollments Table
CREATE TABLE Enrollments (
    EnrollmentID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    CourseID INT NOT NULL,
    EnrollmentDate DATETIME2 DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Active' CHECK (Status IN ('Active', 'Completed', 'Dropped')),
    Notes NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID) ON DELETE CASCADE,
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE,
    UNIQUE(StudentID, CourseID)
);
GO

-- 7. Grades Table (no cascading on multiple paths)
CREATE TABLE Grades (
    GradeID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT NOT NULL,
    CourseID INT NOT NULL,
    TeacherID INT NOT NULL,
    EnrollmentID INT NOT NULL,
    GradeType NVARCHAR(20) NOT NULL CHECK (GradeType IN ('Assignment', 'Midterm', 'Final', 'Participation')),
    Score DECIMAL(5,2) NOT NULL CHECK (Score >= 0 AND Score <= 100),
    Weight DECIMAL(3,2) DEFAULT 1.00 CHECK (Weight >= 0 AND Weight <= 1),
    Comments NVARCHAR(500),
    GradingDate DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    FOREIGN KEY (StudentID) REFERENCES Students(StudentID) ON DELETE NO ACTION,
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE NO ACTION,
    FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID) ON DELETE NO ACTION,
    FOREIGN KEY (EnrollmentID) REFERENCES Enrollments(EnrollmentID) ON DELETE NO ACTION
);
GO
-- 8. ClassRooms Table (Class definitions)
CREATE TABLE ClassRooms (
    ClassRoomID INT IDENTITY(1,1) PRIMARY KEY,
    ClassCode NVARCHAR(20) NOT NULL UNIQUE,
    ClassName NVARCHAR(100) NOT NULL,
    Department NVARCHAR(100) NOT NULL,
    AcademicYear NVARCHAR(10) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL
);

-- 9. ClassCourses Table (Many-to-Many: ClassRooms - Courses)
CREATE TABLE ClassCourses (
    ClassCourseID INT IDENTITY(1,1) PRIMARY KEY,
    ClassRoomID INT NOT NULL,
    CourseID INT NOT NULL,
    AssignedAt DATETIME2 DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1,
    CONSTRAINT UQ_ClassRoom_Course UNIQUE (ClassRoomID, CourseID),
    FOREIGN KEY (ClassRoomID) REFERENCES ClassRooms(ClassRoomID) ON DELETE CASCADE,
    FOREIGN KEY (CourseID) REFERENCES Courses(CourseID) ON DELETE CASCADE
);


-- =============================================
-- Create Indexes
-- =============================================
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Role ON Users(Role);
CREATE INDEX IX_Teachers_TeacherCode ON Teachers(TeacherCode);
CREATE INDEX IX_Students_StudentCode ON Students(StudentCode);
CREATE INDEX IX_Courses_CourseCode ON Courses(CourseCode);
CREATE INDEX IX_Courses_Department ON Courses(Department);
CREATE INDEX IX_CourseAssignments_CourseID ON CourseAssignments(CourseID);
CREATE INDEX IX_CourseAssignments_TeacherID ON CourseAssignments(TeacherID);
CREATE INDEX IX_Enrollments_StudentID ON Enrollments(StudentID);
CREATE INDEX IX_Enrollments_CourseID ON Enrollments(CourseID);
CREATE INDEX IX_Grades_StudentID ON Grades(StudentID);
CREATE INDEX IX_Grades_CourseID ON Grades(CourseID);
CREATE INDEX IX_Grades_TeacherID ON Grades(TeacherID);
GO

-- =============================================
-- Insert Sample Data
-- =============================================
INSERT INTO Users (Username, PasswordHash, FullName, Email, PhoneNumber, Role)
VALUES ('admin', 'admin123', N'System Administrator', 'admin@sims.edu', '0123456789', 'Admin');
GO

INSERT INTO Users (Username, PasswordHash, FullName, Email, PhoneNumber, Role)
VALUES 
('teacher1', 'teacher123', N'Nguyễn Văn A', 'teacher1@sims.edu', '0123456781', 'Teacher'),
('teacher2', 'teacher123', N'Trần Thị B', 'teacher2@sims.edu', '0123456782', 'Teacher'),
('teacher3', 'teacher123', N'Lê Văn C', 'teacher3@sims.edu', '0123456783', 'Teacher');
GO

INSERT INTO Teachers (TeacherCode, UserID, Department, Specialization, AcademicDegree, HireDate)
VALUES 
('T001', 2, N'Khoa Công nghệ thông tin', N'Lập trình Web', N'Thạc sĩ', '2020-01-15'),
('T002', 3, N'Khoa Kinh tế', N'Quản trị kinh doanh', N'Tiến sĩ', '2019-03-20'),
('T003', 4, N'Khoa Ngoại ngữ', N'Tiếng Anh', N'Thạc sĩ', '2021-06-10');
GO

INSERT INTO Users (Username, PasswordHash, FullName, Email, PhoneNumber, Role)
VALUES 
('student1', 'student123', N'Phạm Văn D', 'student1@sims.edu', '0123456791', 'Student'),
('student2', 'student123', N'Hoàng Thị E', 'student2@sims.edu', '0123456792', 'Student'),
('student3', 'student123', N'Vũ Văn F', 'student3@sims.edu', '0123456793', 'Student'),
('student4', 'student123', N'Đỗ Thị G', 'student4@sims.edu', '0123456794', 'Student');
GO

INSERT INTO Students (StudentCode, UserID, Class, Major, EnrollmentDate)
VALUES 
('S001', 5, N'CNTT-K20', N'Công nghệ thông tin', '2020-09-01'),
('S002', 6, N'KT-K20', N'Kinh tế', '2020-09-01'),
('S003', 7, N'CNTT-K21', N'Công nghệ thông tin', '2021-09-01'),
('S004', 8, N'NN-K21', N'Ngôn ngữ Anh', '2021-09-01');
GO

INSERT INTO Courses (CourseCode, CourseName, Credits, Duration, Description, Department, Semester, AcademicYear, MaxStudents, Fee)
VALUES 
('CS101', N'Lập trình cơ bản', 3, N'45 giờ', N'Khóa học cơ bản về lập trình', N'Khoa Công nghệ thông tin', N'Học kỳ 1', '2024-2025', 40, 1500000),
('CS201', N'Lập trình Web', 4, N'60 giờ', N'Khóa học về phát triển web', N'Khoa Công nghệ thông tin', N'Học kỳ 2', '2024-2025', 35, 2000000),
('EC101', N'Kinh tế học đại cương', 3, N'45 giờ', N'Khóa học cơ bản về kinh tế', N'Khoa Kinh tế', N'Học kỳ 1', '2024-2025', 50, 1200000),
('EN101', N'Tiếng Anh cơ bản', 2, N'30 giờ', N'Khóa học tiếng Anh cơ bản', N'Khoa Ngoại ngữ', N'Học kỳ 1', '2024-2025', 60, 800000);
GO

-- Các bảng dữ liệu khác như CourseAssignments, Enrollments, Grades cũng chia batch tương tự
-- Ví dụ:
INSERT INTO CourseAssignments (CourseID, TeacherID, Notes)
VALUES 
(1, 1, N'Phụ trách chính'),
(2, 1, N'Phụ trách chính'),
(3, 2, N'Phụ trách chính'),
(4, 3, N'Phụ trách chính');
GO

INSERT INTO Enrollments (StudentID, CourseID, Status)
VALUES 
(1, 1, 'Active'),
(1, 4, 'Active'),
(2, 3, 'Active'),
(2, 4, 'Active'),
(3, 1, 'Active'),
(3, 2, 'Active'),
(4, 4, 'Active');
GO

INSERT INTO Grades (StudentID, CourseID, TeacherID, EnrollmentID, GradeType, Score, Weight, Comments)
VALUES 
(1, 1, 1, 1, 'Assignment', 85.5, 0.3, N'Bài tập tốt'),
(1, 1, 1, 1, 'Midterm', 78.0, 0.3, N'Thi giữa kỳ'),
(1, 1, 1, 1, 'Final', 82.0, 0.4, N'Thi cuối kỳ'),
(1, 4, 3, 2, 'Assignment', 90.0, 0.4, N'Xuất sắc'),
(1, 4, 3, 2, 'Final', 88.5, 0.6, N'Rất tốt'),
(2, 3, 2, 3, 'Assignment', 75.0, 0.3, N'Cần cải thiện'),
(2, 3, 2, 3, 'Midterm', 80.0, 0.3, N'Khá'),
(2, 3, 2, 3, 'Final', 85.0, 0.4, N'Tốt'),
(3, 1, 1, 5, 'Assignment', 92.0, 0.3, N'Xuất sắc'),
(3, 1, 1, 5, 'Midterm', 88.0, 0.3, N'Rất tốt'),
(3, 1, 1, 5, 'Final', 90.0, 0.4, N'Xuất sắc');
GO

-- =============================================
-- Create Views
-- =============================================
GO
CREATE VIEW vw_StudentDashboard AS
SELECT 
    s.StudentID,
    s.StudentCode,
    u.FullName,
    u.Email,
    s.Class,
    s.Major,
    s.EnrollmentDate,
    COUNT(DISTINCT e.CourseID) as EnrolledCourses,
    AVG(g.Score) as AverageGrade
FROM Students s
JOIN Users u ON s.UserID = u.UserID
LEFT JOIN Enrollments e ON s.StudentID = e.StudentID AND e.Status = 'Active'
LEFT JOIN Grades g ON s.StudentID = g.StudentID
WHERE s.IsActive = 1
GROUP BY s.StudentID, s.StudentCode, u.FullName, u.Email, s.Class, s.Major, s.EnrollmentDate;
GO

CREATE VIEW vw_TeacherDashboard AS
SELECT 
    t.TeacherID,
    t.TeacherCode,
    u.FullName,
    u.Email,
    t.Department,
    COUNT(DISTINCT ca.CourseID) as AssignedCourses,
    COUNT(DISTINCT e.StudentID) as TotalStudents
FROM Teachers t
JOIN Users u ON t.UserID = u.UserID
LEFT JOIN CourseAssignments ca ON t.TeacherID = ca.TeacherID AND ca.IsActive = 1
LEFT JOIN Enrollments e ON ca.CourseID = e.CourseID AND e.Status = 'Active'
WHERE t.IsActive = 1
GROUP BY t.TeacherID, t.TeacherCode, u.FullName, u.Email, t.Department;
GO

CREATE VIEW vw_AdminDashboard AS
SELECT 
    COUNT(*) as TotalUsers,
    COUNT(CASE WHEN Role = 'Student' THEN 1 END) as TotalStudents,
    COUNT(CASE WHEN Role = 'Teacher' THEN 1 END) as TotalTeachers,
    COUNT(CASE WHEN Role = 'Admin' THEN 1 END) as TotalAdmins
FROM Users
WHERE IsActive = 1;
GO

-- =============================================
-- Create Stored Procedures
-- =============================================
GO
CREATE PROCEDURE sp_GetStudentGrades
    @StudentID INT
AS
BEGIN
    SELECT 
        c.CourseCode,
        c.CourseName,
        g.GradeType,
        g.Score,
        g.Weight,
        g.Comments,
        g.GradingDate,
        u.FullName as TeacherName
    FROM Grades g
    JOIN Courses c ON g.CourseID = c.CourseID
    JOIN Teachers t ON g.TeacherID = t.TeacherID
    JOIN Users u ON t.UserID = u.UserID
    WHERE g.StudentID = @StudentID AND g.IsActive = 1
    ORDER BY c.CourseCode, g.GradingDate;
END;
GO

CREATE PROCEDURE sp_GetTeacherCourses
    @TeacherID INT
AS
BEGIN
    SELECT 
        c.CourseCode,
        c.CourseName,
        c.Credits,
        c.Duration,
        c.Department,
        c.Semester,
        c.AcademicYear,
        COUNT(e.StudentID) as EnrolledStudents
    FROM CourseAssignments ca
    JOIN Courses c ON ca.CourseID = c.CourseID
    LEFT JOIN Enrollments e ON c.CourseID = e.CourseID AND e.Status = 'Active'
    WHERE ca.TeacherID = @TeacherID AND ca.IsActive = 1
GROUP BY c.CourseID, c.CourseCode, c.CourseName, c.Credits, c.Duration, c.Department, c.Semester, c.AcademicYear;
END;
GO

PRINT 'SIMS Database created successfully with sample data!';
PRINT 'Sample accounts:';
PRINT 'Admin: admin/admin123';
PRINT 'Teacher: teacher1/teacher123';
PRINT 'Student: student1/student123';
GO
