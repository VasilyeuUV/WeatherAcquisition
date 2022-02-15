using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace WeatherAcquisition.BlazorUI.Infrastructure.Extensions
{

    /// <summary>
    /// Расширения сервисов
    /// </summary>
    internal static class ServiceExtentions
    {
        /// <summary>
        /// Метод расширения для регистрации контроллеров в приложении 
        /// </summary>
        /// <typeparam name="IInterface"></typeparam>
        /// <typeparam name="IClient"></typeparam>
        /// <param name="services"></param>
        /// <param name="adress"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddApi<IInterface, IClient>(this IServiceCollection services, string adress) 
            where IInterface : class where IClient : class, IInterface
            => services
            .AddHttpClient<IInterface, IClient>(
                // Конфигурируем HttpClient (меняем у объекта клиента базовый адрес):
                // - базовый адрес берем из хоста;
                // - добавляем к нему путь Контроллера репозитория источников данных
                (host, client) => client.BaseAddress = new($"{host.GetRequiredService<IWebAssemblyHostEnvironment>().BaseAddress}{adress}")
                );
    }
}
