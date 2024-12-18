
using Microsoft.EntityFrameworkCore;
using WebApplication20.Models;

namespace WebApplication20
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Test1Connection")));

            // Add services to the container
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Add static files middleware (for serving static assets)
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=SinhVien}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
