using System.Threading;
using System.Threading.Tasks;

namespace WeatherAcquisition.Interfaces.Base.Operations
{
    /// <summary>
    /// Получить количество счётных единиц
    /// </summary>
    public interface IGetCountAsync
    {
        /// <summary>
        /// Получить количество
        /// </summary>
        /// <param name="cancel">Возможность отмены операции</param>
        /// <returns>Количество посчитанных единиц</returns>
        Task<long> GetCountAsync(CancellationToken cancel = default);
    }
}
