using Microsoft.Extensions.DependencyInjection;
using WeatherAcquisition.WPF.ViewModels;

namespace WeatherAcquisition.WPF.Services
{
    /// <summary>
    /// Локатор сервисов
    /// </summary>
    internal class ServiceLocator
    {
        /// <summary>
        /// Модель-представление главного окна
        /// </summary>
        public MainWindowViewModel MainWindowViewModel 
            => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
