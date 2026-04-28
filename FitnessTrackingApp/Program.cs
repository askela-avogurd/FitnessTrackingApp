
using FitnessTrackingApp.Services;
using Microsoft.EntityFrameworkCore;
using FitnessTrackingApp.Data;

namespace FitnessTrackingApp
{
    public class Program
    {
        /// <summary>
        /// Точка входа и настройки.
        /// </summary>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Регистрация контекста с SQL Server - настройка строки подключения БД.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Добавление валидатора для активностей (валидация и выдача стикера).
            builder.Services.AddScoped<IActivityService, ActivityService>();

            // Добавление поддержки контроллеров.
            builder.Services.AddControllers();

            // Подключение сваггера для автодокументации.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new()
                {
                    Title = "FitnessApp API",
                    Version = "v1"
                })    
            );
            // Настройка игнорирования цикличных ссылок при связях таблиц в JSON.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            // Настройка конвейера промежуточного ПО.

            var app = builder.Build();

            // Настройка SwaggerUI для тестирования API.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Перенаправление http на https.
            app.UseHttpsRedirection();

            // Подключение контроллеров.
            app.MapControllers();

            // Миграции БД
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate(); // Применяются миграции на старте.
            }

            app.Run();
        }
    }
}
