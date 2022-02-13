using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;
using WeatherAcquisition.WebAPIClients.Clients;

namespace WeatherAcquisition.ConsoleUI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Hosting;
            await host.StartAsync();

            var dataSources = Services.GetRequiredService<IRepository<DataSource>>();   // - получаем сервис репозитория

            
            var count = await dataSources.GetCountAsync();              // - получаем количество записей в репозитории            
            var sources = await dataSources.GetAsync(3,5);              // - пропускаем 3, получаем 5

            /// Добавление источника
            var addedSource = await dataSources.AddAsync(
                new DataSource
                {
                    Name = $"Новый источник {DateTime.Now:HH_mm_ss}",
                    Description = $"Время добавления нового источника: {DateTime.Now:HH_mm_ss}"
                });

            sources = await dataSources.GetAllAsync();                  // - получаем все источники
            foreach (var source in sources)
                Console.WriteLine($"{source.Id}: {source.Name} - {source.Description}");

            var page = await dataSources.GetPageAsync(2, 3);            // - получить вторую страницу размером 3 элемента
            foreach (var item in page.Items)
                Console.WriteLine($"{item.Id}: {item.Name} - {item.Description}");



            Console.WriteLine("Завершено");
            Console.ReadLine();

            await host.StopAsync();
        }


        //########################################################################################
        #region Services

        /// <summary>
        /// Сервисы
        /// </summary>
        public static IServiceProvider Services => Hosting.Services;

        /// <summary>
        /// Конфигуратор сервисов
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            // РЕГИСТРАЦИЯ СЕРВИСОВ

            // - сервис получения данных о погоде
            //services.AddHttpClient<MetaWeatherClient>(client =>                     // - конфигурация сервиса
            //    client.BaseAddress = new Uri(host.Configuration["MetaWeather"]))    // - получаем базовый URL
            //    .SetHandlerLifetime(TimeSpan.FromMinutes(5))                        // - время жизни клиента (нужны установленные расширения Ms.Ext.Http.Polly и Polly.Extentions.Http)
            //    .AddPolicyHandler(GetRetryPolicy())                                 // - дополнительный политика клиента
            //    ;

            // HttpClient будет работать как сервис приложения
            services.AddHttpClient<IRepository<DataSource>, WebRepositoryClient<DataSource>>(   
                client =>
                {
                    // - получаем из конфигурации базовый адрес и добавляем адрес самого контроллера
                    // в конце обязательно слэш / !!! Без него работать не будет
                    client.BaseAddress = new Uri($"{host.Configuration["WebAPI"]}/api/DataSources/");
                });      

        }


        #endregion // Services


        //########################################################################################
        #region Host

        private static IHost __hosting;
        /// <summary>
        /// Host
        /// </summary>
        public static IHost Hosting => __hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        /// <summary>
        /// Построитель хоста
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);          // конфигурируем сервисы (контейнер сервисов)


        ///// <summary>
        ///// Политики клиента
        ///// </summary>
        ///// <returns></returns>
        //private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        //{
        //    // Джиттер - для обеспечение возможности рассинхронизации доступа
        //    // при большом количестве запросов на сервер от разных клиентов
        //    var jitter = new Random();

        //    return HttpPolicyExtensions
        //        .HandleTransientHttpError()                                         // - перехватываем возможные ошибки процесса соединения с сервером
        //        .WaitAndRetryAsync(                                                 // - если сервер недоступен, то Http повторит запрос
        //            6,                                                              // -- 6 раз
        //            retry_attempt =>                                                // -- номер повтора
        //                TimeSpan.FromSeconds(Math.Pow(2, retry_attempt)) +          // --- через время с прогрессией в 2 секунды в зависимости от номера повтора
        //                TimeSpan.FromMilliseconds(jitter.Next(0, 1000)))            // --- со случайной рассинхронизацией времени запроса в 1 миллисекунду
        //        ;
        //}

        #endregion // Hosting

    }
}
