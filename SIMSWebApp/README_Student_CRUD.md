# Student CRUD Functionality User Guide

## Overview
The CRUD (Create, Read, Update, Delete) functionality for Students has been integrated into the SIMS Web App system, using SQL Server Management Studio 20 as the database.

## Main Features

### 1. Add New Student (Create)
- **Route**: `/Student/Create`
- **Features**: 
  - Enter new student information
  - Auto-generate student code from full name
  - Input data validation
  - Check for duplicate student codes

### 2. View Student List (Read)
- **Route**: `/Student/Index`
- **Features**:
  - Display all students
  - Sort by name
  - Search and filter data
  - Pagination with DataTables
  - Action buttons: View, Edit, Delete

### 3. View Student Details (Read)
- **Route**: `/Student/Details/{id}`
- **Features**:
  - Display complete student information
  - Quick edit button
  - Creation and update timestamps

### 4. Edit Student (Update)
- **Route**: `/Student/Edit/{id}`
- **Features**:
  - Edit student information
  - Data validation
  - Check for duplicate student codes
  - Update modification timestamp

### 5. Delete Student (Delete)
- **Route**: `/Student/Delete/{id}`
- **Features**:
  - Soft delete (marks as inactive, doesn't remove data)
  - Confirmation before deletion
  - Preserves data integrity

## Database Structure

### Students Table
```sql
CREATE TABLE Students (
    StudentID int IDENTITY(1,1) PRIMARY KEY,
    FullName nvarchar(100) NOT NULL,
    StudentCode nvarchar(20) NOT NULL UNIQUE,
    DateOfBirth date NOT NULL,
    Gender nvarchar(10) NOT NULL,
    Address nvarchar(200) NOT NULL,
    PhoneNumber nvarchar(15) NOT NULL,
    Email nvarchar(100) NULL,
    Class nvarchar(20) NOT NULL,
    CreatedAt datetime2(7) NOT NULL DEFAULT GETDATE(),
    UpdatedAt datetime2(7) NULL,
    IsActive bit NOT NULL DEFAULT 1
)
```

### Indexes
- `IX_Students_StudentCode`: Unique index for student code
- `IX_Students_FullName`: Index for student name
- `IX_Students_Class`: Index for class

## Installation and Setup

### 1. Create Database
1. Open SQL Server Management Studio 20
2. Connect to SQL Server instance
3. Run script: `Database/Scripts/CreateStudentsTable.sql`
4. Verify Students table was created successfully

### 2. Run Application
1. Open project in Visual Studio
2. Build project
3. Run application
4. Navigate to `/Student` to use functionality

### 3. Check Database Connection
- Verify connection string in `appsettings.json`
- Ensure SQL Server is running
- Check database access permissions

## Code Structure

### Models
- `StudentViewModel`: ViewModel for forms and display
- `Student`: Entity model

### Controllers
- `StudentController`: Handles HTTP requests

### Services
- `StudentService`: Business logic for Student

### Repositories
- `IStudentRepository`: Interface defining methods
- `StudentRepository`: Repository implementation

### Views
- `Index.cshtml`: Student list
- `Create.cshtml`: Add new form
- `Edit.cshtml`: Edit form
- `Details.cshtml`: Detail display

## Validation and Security

### Validation
- Full Name: Required, max 100 characters
- Student Code: Required, max 20 characters, unique
- Date of Birth: Required, date format
- Gender: Required, select from list
- Address: Required, max 200 characters
- Phone Number: Required, phone format, max 15 characters
- Email: Optional, email format, max 100 characters
- Class: Required, max 20 characters

### Security
- CSRF protection with `[ValidateAntiForgeryToken]`
- Input validation and sanitization
- Soft delete to preserve data

## Additional Features

### Auto-generated Student Codes
- When entering full name, system auto-generates student code
- Format: [First letter of last name][First letter of first name][Year][Sequence number]

### DataTables Integration
- Search and filter data
- Pagination
- Column sorting
- English language support

### Responsive Design
- Mobile-friendly interface
- Bootstrap 5 components
- Font Awesome icons

## Error Handling

### Common Errors
1. **Database connection error**: Check connection string and SQL Server
2. **Validation error**: Check input data
3. **Duplicate key error**: Student code already exists
4. **Not found error**: Student doesn't exist

### Logging
- Errors logged to console
- Try-catch exception handling
- User-friendly error messages

## Feature Extensions

### Can Add
- Import/Export Excel
- Advanced search
- Statistical reports
- Student photo upload
- Grade management
- Class management

### API Endpoints
- RESTful API for mobile app
- JSON response format
- Authentication and authorization

## Support

If you encounter issues, please check:
1. Connection string in `appsettings.json`
2. SQL Server service is running
3. SIMS database has been created
4. Students table has been created with SQL script
5. Application console logs
