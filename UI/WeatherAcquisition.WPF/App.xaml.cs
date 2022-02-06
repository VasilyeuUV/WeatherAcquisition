using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Windows;
using WeatherAcquisition.WPF.ViewModels;
using WeatherAcquisition.WPF.Views.Windows;
using WpfMvvmBase.UIServices.UserDialogService;

namespace WeatherAcquisition.WPF
{
    public partial class App
    {

        //#############################################################################
        #region Возможность доступа к окнам

        /// <summary>
        /// Текущее окно приложения
        /// </summary>
        public static Window? CurrentWindow => FocusedWindow ?? ActiveWindow;

        /// <summary>
        /// Активное окно приложения
        /// </summary>
        public static Window? ActiveWindow =>
            Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);

        /// <summary>
        /// Окно с фокусом ввода
        /// </summary>
        public static Window? FocusedWindow =>
            Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);

        #endregion // Возможность доступа к окнам



        //#############################################################################
        #region Host

        private static IHost? __hosting;

        public static IHost Hosting => __hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices)
            ;


        /// <summary>
        /// Старт хостинга
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Hosting;
            base.OnStartup(e); 
            await host.StartAsync().ConfigureAwait(true);
            //Services.GetRequiredService<MainWindow>().Show();
        }

        /// <summary>
        /// Остановка хостинга
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnExit(ExitEventArgs e)
        {
            using var host = Hosting;
            base.OnExit(e);
            await host.StopAsync().ConfigureAwait(false);
        }

        #endregion // Host


        //#############################################################################
        #region Services

        public static IServiceProvider Services => Hosting.Services;

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddScoped<MainWindowViewModel>()   // регистрируем модель-представление главного окна
                ;
        }

        #endregion // Services

    }
}
