using FDRWebsite.Server.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Npgsql;

namespace FDRWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication()
                .AddJwtBearer();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddRepositories();
            builder.Services.AddSwaggerGen();

            builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            builder.Configuration.AddJsonFile("appsettings.json");

            // Add database service
            var connectionString = builder.Configuration.GetConnectionString("ProstgreSQLConnectionString");
            builder.Services.AddScoped((provider) => {
                var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                return conn;
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}