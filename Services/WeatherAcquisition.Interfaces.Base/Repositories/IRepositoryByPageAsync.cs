using System.Collections.Generic;
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
        /// <returns></returns>
        Task<IEnumerable<T>> GetAsync(long skip, int count);


        /// <summary>
        /// Получение страницы с данными
        /// </summary>
        /// <param name="pageIndex">Индекс страницы</param>
        /// <param name="pageSize">размер страницы</param>
        /// <returns>Страница с данными</returns>
        Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize);
    }
}
