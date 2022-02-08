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
            // ���������� �������� ���� ������
            services.AddDbContext<DataBaseContext>(                     
                opt => opt
                .UseSqlServer(                                                      // - ��������� SQLServer
                    configuration.GetConnectionString("DataConnectionString"),      // --  ������ �����������
                    o => o.MigrationsAssembly("WeatherAcquisition.DAL.SqlServer")   // -- ��������� �������� � ��������� �������,
                                                                                    // � ������� ��� ����� ���������
                    ));

            /* ��� ������� ��������:
             - ������� ����������� ������ �� �������� ��������� �������� (WeatherAcquisition.API);
             - ��������� ������� ������������ ��������:
               � Microsoft.EntityFrameworkCore.Design;
             - ������� � ���� "������� ���������� �������";
             - � �� ������������� �� ������ (������ �� ���������), ��� ����� ������ �������� (WeatherAcquisition.DAL.SqlServer);
             - �������� ��������: Add-Migration Init

               ��� ���������� ��������
               (����� ��������� �������, ������� ���� ������)
              - ������� ����������� ������ �� �������� ��������� �������� (WeatherAcquisition.API);
              - ������� � ���� "������� ���������� �������";
              - � �� ������������� �� ������, � ������� ����� �������� (WeatherAcquisition.DAL.SqlServer);
              - �������� ��������: Add-Migration =��������_��������=
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
