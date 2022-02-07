using System.Threading.Tasks;

namespace WeatherAcquisition.Interfaces.Base.Operations
{
    /// <summary>
    /// Получить количество счётных единиц
    /// </summary>
    public interface IGetCountAsync
    {
        /// <summary>
        /// Полученить количество
        /// </summary>
        /// <returns></returns>
        Task<long> GetCountAsync();
    }
}
