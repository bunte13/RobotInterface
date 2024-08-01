using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RobotInterface.Hubs; // Add this line
using RobotInterface.Data;
using RobotInterface.Services;
using Microsoft.EntityFrameworkCore;

namespace RobotInterface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure DbContext
            builder.Services.AddDbContext<RobotInterfaceContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RobotInterfaceContext")
                    ?? throw new InvalidOperationException("Connection string 'RobotInterfaceContext' not found.")));

            // Set EPPlus license context
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Register services
            builder.Services.AddScoped<IRobotService, RobotService>();
            builder.Services.AddScoped<ISshService, SshService>();
            builder.Services.AddSingleton<ISshStateService, SshStateService>();
            builder.Services.AddSingleton<IWebSocketService>(provider =>
                new WebSocketService(builder.Configuration["WebSocket:Url"]));

            // Add SignalR service
            builder.Services.AddSignalR();

            // Add MVC services
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 7015;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Functions}/{action=Index}/{id?}");

            // Map the SignalR hub
            app.MapHub<RobotHub>("/robotHub");

            app.Run();
        }
    }
}
