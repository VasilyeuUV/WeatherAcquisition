using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WeatherAcquisition.API.Data;
using WeatherAcquisition.DAL.Contexts;

namespace WeatherAcquisition.API
{
    public record Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Подключаем контекст базы данных
            services.AddDbContext<DataBaseContext>(                     
                opt => opt
                .UseSqlServer(                                                      // - добавляем SQLServer
                    configuration.GetConnectionString("DataConnectionString"),      // --  строка подключения
                    o => o.MigrationsAssembly("WeatherAcquisition.DAL.SqlServer")   // -- добавляем миграции с указанием проекта,
                                                                                    // в котором они будут находится
                    ));

            // Подключаем инициализатор БД
            services.AddTransient<DataBaseContextInitializer>();


            /* ДЛЯ ЗАПУСКА МИГРАЦИИ:
              - сделать запускаемым проект из которого запускаем миграции (WeatherAcquisition.API);
              - проверить наличие подключенных проектов:
                • Microsoft.EntityFrameworkCore.Design;
              - перейти в окно "Консоль диспетчера пакетов";
              - в нём переключиться на проект (Проект по умолчанию), где будут лежать миграции (WeatherAcquisition.DAL.SqlServer);
              - добавить миграцию: Add-Migration Init

               ДЛЯ ОБНОВЛЕНИЯ МИГРАЦИИ
               (после изменения чвойств, классов базы данных)
               - сделать запускаемым проект из которого запускаем миграции (WeatherAcquisition.API);
               - перейти в окно "Консоль диспетчера пакетов";
               - в нём переключиться на проект, в котором лежат миграции (WeatherAcquisition.DAL.SqlServer);
               - добавить миграцию: Add-Migration =название_миграции= -v

               ЧТОБЫ ОТКАТИТЬ ПОСЛЕДНИЕ МИГРАЦИЮ
               - при тех же условиях Remove-Migration

               ДЛЯ ПРИМЕНЕНИЯ МИГРАЦИИ К БАЗЕ ДАННЫХ
               - при тех же условиях Update-Database
               - посмотреть можно в обозревателе объектов SQL Server

               ОТКАТИТЬ ПРИМЕНЕНИЕ МИГРАЦИИ К БАЗЕ ДАННЫХ
               - при тех же условиях Update-Database с названием миграции, к которой откатить
               - откатить миграции до той, к которой привели базу даненых
            */

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAcquisition.API", Version = "v1" });
            });
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            DataBaseContextInitializer dbInitializer
            )
        {
            // Инициализируем БД
            dbInitializer.Initialize();         

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAcquisition.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
