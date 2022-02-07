using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.Repositories
{
    /// <summary>
    /// Репозиторий для постраничного извлечения данных
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IRepositoryByPageAsync<T> where T : IEntity
    {
        /// <summary>
        /// Получение указанного количества сущностей
        /// </summary>
        /// <param name="skip">Количество пропускаемых сущностей</param>
        /// <param name="count">Количество получаемых сущностей</param>
        /// <param name="cancel">Возможность отмены операции</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default);


        /// <summary>
        /// Получение страницы с данными
        /// </summary>
        /// <param name="pageIndex">Индекс страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="cancel">Возможность отмены операции</param>
        /// <returns>Страница с данными</returns>
        Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default);
    }
}
