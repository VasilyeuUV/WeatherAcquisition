using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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
              - добавить миграцию: Add-Migration =название_миграции=
            */

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAcquisition.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
