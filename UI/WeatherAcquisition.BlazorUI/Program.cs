using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeatherAcquisition.BlazorUI;
using WeatherAcquisition.BlazorUI.Infrastructure.Extensions;
using WeatherAcquisition.Domain.Base.Models;
using WeatherAcquisition.Interfaces.Base.Repositories;
using WeatherAcquisition.WebAPIClients.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");    // #app - идентификатор блока из index.html, где будет отображатьс€ приложение
builder.RootComponents.Add<HeadOutlet>("head::after");

var services = builder.Services;
services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Ёто повтор€ем дл€ всех репозиториев, которые будут в приложении
//services.AddHttpClient<IRepository<DataSourceDomainBaseModel>, WebRepositoryClient<DataSourceDomainBaseModel>>(
//    //  онфигурируем HttpClient (мен€ем у объекта клиента базовый адрес):
//    // - базовый адрес берем из хоста;
//    // - добавл€ем к нему путь  онтроллера репозитори€ источников данных
//    (host, client) => client.BaseAddress = new(host.GetRequiredService<IWebAssemblyHostEnvironment>().BaseAddress + "api/DataSourceRepository")  
//    );

// ”прощение регистрации контроллеров репозиториев
services.AddApi<IRepository<DataSourceDomainBaseModel>, WebRepositoryClient<DataSourceDomainBaseModel>>("api/DataSourceRepository");

await builder.Build().RunAsync();
