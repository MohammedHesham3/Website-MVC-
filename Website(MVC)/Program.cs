using Microsoft.EntityFrameworkCore;
using Website_MVC_.Data;
using Website_MVC_.Models;

namespace Website_MVC_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            // Use In-Memory Database for Azure deployment
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("FoodTrackerDb"));
            
            // Add Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add CORS for frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.AllowAnyOrigin()  // For now, allow all origins
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            ////

            var app = builder.Build();

            // Seed data on startup for in-memory DB
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                
                // Manually seed the Apple data if it doesn't exist
                if (!context.Foods.Any())
                {
                    context.Foods.Add(new Food
                    {
                        FoodId = 1,
                        Name = "Apple",
                        ServingSize = 100,
                        Calories = 52,
                        Protein = 0.3f,
                        Carbs = 13.8f,
                        Fat = 0.2f
                    });
                    
                    // Add a few more sample foods for testing
                    context.Foods.Add(new Food
                    {
                        FoodId = 2,
                        Name = "Banana",
                        ServingSize = 120,
                        Calories = 89,
                        Protein = 1.1f,
                        Carbs = 22.8f,
                        Fat = 0.3f
                    });
                    
                    context.Foods.Add(new Food
                    {
                        FoodId = 3,
                        Name = "Chicken Breast",
                        ServingSize = 100,
                        Calories = 165,
                        Protein = 31.0f,
                        Carbs = 0.0f,
                        Fat = 3.6f
                    });
                    
                    context.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable Swagger middleware (in all environments)
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nutrition API V1");
                c.RoutePrefix = string.Empty; // Swagger at root
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable CORS - MUST be between UseRouting and UseAuthorization
            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
