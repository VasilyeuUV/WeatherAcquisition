using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WeatherAcquisition.API.Data;
using WeatherAcquisition.DAL.Contexts;
using WeatherAcquisition.DAL.Repositories;
using WeatherAcquisition.Interfaces.Base.Repositories;

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

            // ���������� ������������� ��
            services.AddTransient<DataBaseContextInitializer>();

            // ���������� �����������
            //// ������� 1 - ������. ���������� ������ ��������
            //services.AddScoped<IRepository<DataSource>, DbRepository<DataSource>>();
            //services.AddScoped<IRepository<DataValue>, DbRepository<DataValue>>();
            // ������� 2 - ����������.
            // ���������� ������, ������� ������������ ����� ����������, �� ��������, ��� ����� ��������� ��������
            // �.�. � ��������� �������� ����� �������������� ����������� (���������) �������,
            // � �������� � ����������� ��������� �������� ����� ��� ����������� T
            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));              // - ������������ ������� �����������
            services.AddScoped(typeof(INamedRepository<>), typeof(DbNamedRepository<>));    // - ������������ ����������� �����������


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
               - �������� ��������: Add-Migration =��������_��������= -v

               ����� �������� ��������� ��������
               - ��� ��� �� �������� Remove-Migration

               ��� ���������� �������� � ���� ������
               - ��� ��� �� �������� Update-Database
               - ���������� ����� � ������������ �������� SQL Server

               �������� ���������� �������� � ���� ������
               - ��� ��� �� �������� Update-Database � ��������� ��������, � ������� ��������
               - �������� �������� �� ���, � ������� ������� ���� �������
            */

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeatherAcquisition.API", Version = "v1" });
            });

            // - ���������� ����������
            services.AddAutoMapper(typeof(Startup));        // ����� ��� Startup, ������ �� ���� ������, ������������ ��
                                                            // � ����� ��� ����������� ��� ������ �������
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            DataBaseContextInitializer dbInitializer
            )
        {
            // �������������� ��
            dbInitializer.Initialize();         

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();      // ����������� � ��������, ����� ����� ���� ���������� ��, ��� ��������� � ��������
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeatherAcquisition.API v1"));
            }

            app.UseBlazorFrameworkFiles();  // ���������� �������������� (����� �����) Blazor
            app.UseStaticFiles();           // ����� ���������� ����������� ����� ��� ������ ������ � ��������

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");  // ��, ��� �� ������������ ��� ���������� ��� ������� (���������� �����)
                                                            // ����� �������������� �� ���� index.html
                                                            // ������� ��������� � Blazor-����������
            });
        }
    }
}
