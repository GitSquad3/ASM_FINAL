# SIMS - Student Information Management System

## Overview
SIMS is a comprehensive Student Information Management System built with ASP.NET Core MVC and SQL Server. The system provides role-based access control with three distinct user roles: Admin, Teacher, and Student.

## System Architecture

### Technology Stack
- **Backend**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server 2022
- **ORM**: Entity Framework Core
- **Authentication**: Cookie-based Authentication
- **Authorization**: Role-based Authorization
- **Frontend**: Bootstrap 5, FontAwesome, jQuery

### Database Design
The system uses a normalized database design with the following key tables:

1. **Users** - Central authentication and user management
2. **Teachers** - Teacher-specific information (linked to Users)
3. **Students** - Student-specific information (linked to Users)
4. **Courses** - Course information
5. **CourseAssignments** - Many-to-many relationship between Teachers and Courses
6. **Enrollments** - Many-to-many relationship between Students and Courses
7. **Grades** - Grade information linking Students, Courses, Teachers, and Enrollments

## Role-Based Functionality

### 🔧 Admin
- **User Management**: Create, edit, and manage all user accounts
- **Student Management**: Manage student information and enrollments
- **Teacher Management**: Manage teacher information and assignments
- **Course Management**: Create and manage courses
- **Course Assignments**: Assign teachers to courses
- **Enrollment Management**: Manage student course enrollments
- **Grade Management**: View and manage all grades
- **System Overview**: Dashboard with comprehensive statistics

### 👨‍🏫 Teacher
- **Course Management**: View assigned courses
- **Student Management**: View students enrolled in courses
- **Grade Management**: Enter and edit student grades
- **Grade Reports**: View grade statistics and reports
- **Dashboard**: Overview of teaching responsibilities

### 👨‍🎓 Student
- **Profile Management**: View and update personal information
- **Course Information**: View enrolled courses and course details
- **Grade Information**: View grades and academic performance
- **Transcript**: View complete academic transcript
- **Dashboard**: Overview of academic progress

## Installation and Setup

### Prerequisites
- SQL Server 2019 or later
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Database Setup
1. Open SQL Server Management Studio
2. Run the script: `SIMSWebApp/Database/Scripts/CreateDatabase_New.sql`
3. This will create the database with all tables, indexes, views, and sample data

### Application Setup
1. Clone the repository
2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "default": "Server=YOUR_SERVER;Database=SIMS_DB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```
3. Build and run the application
4. Navigate to the application URL

### Sample Accounts
The system comes with pre-configured sample accounts:

| Role | Username | Password |
|------|----------|----------|
| Admin | admin | admin123 |
| Teacher | teacher1 | teacher123 |
| Student | student1 | student123 |

## Key Features

### 🔐 Security
- Role-based access control
- Cookie-based authentication
- Secure password handling
- Session management

### 📊 Dashboard
- Role-specific dashboards
- Real-time statistics
- Quick action buttons
- Responsive design

### 📈 Grade Management
- Multiple grade types (Assignment, Midterm, Final, Participation)
- Weighted grading system
- Grade history tracking
- Performance analytics

### 🎓 Course Management
- Flexible course assignment system
- Student enrollment tracking
- Course capacity management
- Academic year and semester support

### 📋 Reporting
- Student transcripts
- Grade reports
- Course statistics
- Enrollment reports

## Database Views and Stored Procedures

### Views
- **vw_StudentDashboard**: Student dashboard data
- **vw_TeacherDashboard**: Teacher dashboard data
- **vw_AdminDashboard**: Admin dashboard data

### Stored Procedures
- **sp_GetStudentGrades**: Get student grades with details
- **sp_GetTeacherCourses**: Get teacher's assigned courses

## File Structure

```
SIMSWebApp/
├── Controllers/
│   ├── AuthController.cs          # Authentication
│   ├── AdminController.cs         # Admin functionality
│   ├── TeacherController.cs       # Teacher functionality
│   └── StudentController.cs       # Student functionality
├── DatabaseContext/
│   ├── Entities/                  # Entity Framework entities
│   └── SIMSDbContext.cs          # Database context
├── Models/
│   ├── LoginViewModel.cs          # Login form model
│   └── DashboardViewModel.cs      # Dashboard data model
├── Views/
│   ├── Auth/                      # Authentication views
│   ├── Admin/                     # Admin views
│   ├── Teacher/                   # Teacher views
│   └── Student/                   # Student views
└── Database/
    └── Scripts/
        └── CreateDatabase_New.sql # Database creation script
```

## Development Guidelines

### Code Standards
- Follow C# naming conventions
- Use async/await for database operations
- Implement proper error handling
- Use dependency injection
- Follow MVC pattern

### Database Guidelines
- Use proper indexing for performance
- Implement foreign key constraints
- Use appropriate data types
- Follow normalization principles

### Security Guidelines
- Validate all user inputs
- Use parameterized queries
- Implement proper authorization
- Secure sensitive data

## Troubleshooting

### Common Issues
1. **Database Connection**: Ensure SQL Server is running and connection string is correct
2. **Authentication**: Clear browser cookies if login issues occur
3. **Permissions**: Ensure database user has appropriate permissions
4. **Build Errors**: Ensure .NET 8.0 SDK is installed

### Performance Optimization
- Use database indexes for frequently queried columns
- Implement caching for static data
- Optimize database queries
- Use pagination for large datasets

## Future Enhancements

### Planned Features
- Email notifications
- File upload for assignments
- Advanced reporting
- Mobile application
- API endpoints
- Multi-language support

### Technical Improvements
- Implement unit testing
- Add logging framework
- Performance monitoring
- Automated deployment
- Database migration scripts

## Support

For technical support or questions:
1. Check the documentation
2. Review the code comments
3. Check the BUGFIXES.md file
4. Contact the development team

## License

This project is licensed under the MIT License - see the LICENSE file for details.

---

**SIMS - Empowering Education Management** 🎓
