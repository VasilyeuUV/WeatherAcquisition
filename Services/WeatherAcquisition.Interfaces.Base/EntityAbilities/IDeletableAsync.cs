using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.EntityAbilities
{
    /// <summary>
    /// Сущность может быть удалена
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IDeletableAsync<T> where T : IEntity
    {

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="item">Удаляемая сущность</param>
        /// <param name="cancel">Возможность прервать операцию</param>
        /// <returns>Возвращает удалённую сущность или пустую ссылку</returns>
        Task<T> DeleteAsync(T item, CancellationToken cancel = default);


        /// <summary>
        /// Удаление сущности по ее Id
        /// </summary>
        /// <param name="id">Удаляемая сущность</param>
        /// <param name="cancel">Возможность прервать операцию</param>
        /// <returns>Возвращает удалённую сущность или пустую ссылку</returns>
        Task<T> DeleteByIdAsync(Guid id, CancellationToken cancel = default);


    }
}
