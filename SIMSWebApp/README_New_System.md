# SIMS - Student Information Management System

## Tổng quan hệ thống

Hệ thống SIMS được thiết kế với 3 role chính:

### 1. Admin (Quản trị viên)
- **Quyền hạn**: Quản lý tất cả (tài khoản, khóa học, lớp học, sinh viên, phân công giảng viên)
- **Chức năng chính**:
  - Quản lý người dùng (Users)
  - Quản lý sinh viên (Students)
  - Quản lý giảng viên (Teachers)
  - Quản lý khóa học (Courses)
  - Phân công giảng viên cho khóa học (Course Assignments)
  - Quản lý đăng ký khóa học (Enrollments)
  - Xem báo cáo điểm số (Grades)

### 2. Teacher (Giảng viên)
- **Quyền hạn**: Nhập điểm cho sinh viên
- **Chức năng chính**:
  - Xem danh sách khóa học được phân công
  - Xem danh sách sinh viên trong khóa học
  - Nhập và chỉnh sửa điểm số
  - Xem báo cáo điểm của khóa học

### 3. Student (Sinh viên)
- **Quyền hạn**: Xem thông tin cá nhân, lớp, khóa học, điểm
- **Chức năng chính**:
  - Xem thông tin cá nhân
  - Xem danh sách khóa học đã đăng ký
  - Xem điểm số các môn học
  - Xem bảng điểm (Transcript)

## Cấu trúc Database

### Các bảng chính:
1. **Users** - Thông tin tài khoản người dùng
2. **Teachers** - Thông tin giảng viên
3. **Students** - Thông tin sinh viên
4. **Courses** - Thông tin khóa học
5. **CourseAssignments** - Phân công giảng viên cho khóa học
6. **Enrollments** - Đăng ký khóa học của sinh viên
7. **Grades** - Điểm số của sinh viên

### Relationships:
- User ↔ Teacher (1:1)
- User ↔ Student (1:1)
- Teacher ↔ CourseAssignment (1:N)
- Course ↔ CourseAssignment (1:N)
- Student ↔ Enrollment (1:N)
- Course ↔ Enrollment (1:N)
- Enrollment ↔ Grade (1:N)

## Hướng dẫn cài đặt

### 1. Tạo Database
Chạy script SQL trong file `Database/Scripts/CreateDatabase.sql` trong SQL Server Management Studio.

### 2. Cấu hình Connection String
Cập nhật connection string trong `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "default": "Server=your_server;Database=SIMS_DB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 3. Chạy ứng dụng
```bash
dotnet run
```

## Tài khoản mẫu

### Admin
- Username: `admin`
- Password: `admin123`

### Giảng viên
- Username: `teacher1`
- Password: `teacher123`

- Username: `teacher2`
- Password: `teacher123`

### Sinh viên
- Username: `student1`
- Password: `student123`

- Username: `student2`
- Password: `student123`

## Luồng hoạt động

### 1. Đăng nhập
- Truy cập `/Auth/Login`
- Nhập username và password
- Hệ thống sẽ chuyển hướng theo role:
  - Admin → `/Admin/Index`
  - Teacher → `/Teacher/Index`
  - Student → `/Student/Index`

### 2. Admin Workflow
1. Tạo tài khoản cho giảng viên và sinh viên
2. Thêm thông tin giảng viên và sinh viên
3. Tạo khóa học
4. Phân công giảng viên cho khóa học
5. Quản lý đăng ký khóa học

### 3. Teacher Workflow
1. Xem danh sách khóa học được phân công
2. Xem danh sách sinh viên trong khóa học
3. Nhập điểm cho sinh viên
4. Chỉnh sửa điểm nếu cần

### 4. Student Workflow
1. Xem thông tin cá nhân
2. Xem danh sách khóa học đã đăng ký
3. Xem điểm số các môn học
4. Xem bảng điểm tổng hợp

## Tính năng bảo mật

- **Authentication**: Cookie-based authentication
- **Authorization**: Role-based authorization
- **Session Management**: 8 giờ timeout với sliding expiration
- **Access Control**: Mỗi role chỉ có thể truy cập chức năng được phép

## Cấu trúc thư mục

```
SIMSWebApp/
├── Controllers/
│   ├── AuthController.cs          # Xử lý đăng nhập/đăng xuất
│   ├── AdminController.cs         # Chức năng Admin
│   ├── TeacherController.cs       # Chức năng Teacher
│   └── StudentController.cs       # Chức năng Student
├── DatabaseContext/
│   ├── Entities/                  # Các entity models
│   └── SIMSDbContext.cs          # Database context
├── Models/                        # ViewModels
├── Views/                         # Razor views
└── Database/
    └── Scripts/
        └── CreateDatabase.sql     # Script tạo database
```

## Lưu ý quan trọng

1. **Password Security**: Trong môi trường production, cần mã hóa password thay vì lưu plain text
2. **Data Validation**: Cần thêm validation cho tất cả input
3. **Error Handling**: Cần xử lý exception một cách đầy đủ
4. **Logging**: Cần thêm logging cho các hoạt động quan trọng
5. **Backup**: Cần có kế hoạch backup database định kỳ
