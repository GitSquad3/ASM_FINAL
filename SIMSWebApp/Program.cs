using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.Interfaces;
using SIMSWebApp.Repositories;
using SIMSWebApp.Services;

namespace SIMSWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // cau hinh ket noi toi CSDL
            builder.Services.AddDbContext<SIMSDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
            );
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserService>();
            
            // Đăng ký Student services
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<StudentService>();
            
            // Đăng ký Course services
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<CourseService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
                options.AddPolicy("FacultyOnly", policy => policy.RequireRole("Faculty"));
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Authentication must come before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
