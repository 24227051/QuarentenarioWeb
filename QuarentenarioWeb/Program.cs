using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;

namespace QuarentenarioWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("QuarentanarioDB")
                ?? throw new InvalidOperationException("Connection string"
                + "'DefaultConnection' not found.");

            builder.Services.AddDbContext<QuarentenarioContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registro do Identity com suporte a Razor Pages (Default UI)
            //builder.Services.AddDefaultIdentity(options => {
            //    options.SignIn.RequireConfirmedAccount = false; // Ajuste conforme necessário
            //})
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Define a pol’tica de autoriza‹o
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireLabRole", policy => policy.RequireRole("Lab"));
            });

            // Add services to the container.
            builder.Services.AddRazorPages(options =>
            {
                // Limita toda a pasta "Admin" para usu‡rios com a role "Admin"
                options.Conventions.AuthorizeFolder("/Analises", "RequireLabRole");
                options.Conventions.AuthorizeFolder("/AnalisesDetalhes", "RequireLabRole");
                options.Conventions.AuthorizeFolder("/Anexos", "RequireLabRole");
                options.Conventions.AuthorizeFolder("/Clientes", "RequireLabRole");
                options.Conventions.AuthorizeFolder("/Materiais", "RequireLabRole");
                options.Conventions.AuthorizeFolder("/MateriaisPatogenos", "RequireLabRole");
                options.Conventions.AuthorizeFolder("/Patogenos", "RequireLabRole");
                options.Conventions.AuthorizeFolder("/TiposControle", "RequireLabRole");
                options.Conventions.AuthorizeFolder("/TiposPatogeno", "RequireLabRole");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
