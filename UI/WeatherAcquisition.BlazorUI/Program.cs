using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeatherAcquisition.BlazorUI;
using WeatherAcquisition.BlazorUI.Infrastructure.Extensions;
using WeatherAcquisition.Domain.Base.Models;
using WeatherAcquisition.Interfaces.Base.Repositories;
using WeatherAcquisition.WebAPIClients.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");    // #app - ������������� ����� �� index.html, ��� ����� ������������ ����������
builder.RootComponents.Add<HeadOutlet>("head::after");

var services = builder.Services;
services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// ��� ��������� ��� ���� ������������, ������� ����� � ����������
//services.AddHttpClient<IRepository<DataSourceDomainBaseModel>, WebRepositoryClient<DataSourceDomainBaseModel>>(
//    // ������������� HttpClient (������ � ������� ������� ������� �����):
//    // - ������� ����� ����� �� �����;
//    // - ��������� � ���� ���� ����������� ����������� ���������� ������
//    (host, client) => client.BaseAddress = new(host.GetRequiredService<IWebAssemblyHostEnvironment>().BaseAddress + "api/DataSourceRepository")  
//    );

// ��������� ����������� ������������ ������������
services.AddApi<IRepository<DataSourceDomainBaseModel>, WebRepositoryClient<DataSourceDomainBaseModel>>("api/DataSourceRepository");

await builder.Build().RunAsync();
