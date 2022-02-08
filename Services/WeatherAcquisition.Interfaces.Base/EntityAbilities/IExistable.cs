using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.EntityAbilities
{
    /// <summary>
    /// Сущность может существовать
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IExistable<T> where T : IEntity
    {
        /// <summary>
        /// Проверка на существование сущности 
        /// </summary>
        /// <param name="item">Сущность</param>
        /// <param name="cancel">Возможность прервать операцию</param>
        /// <returns>Возвращает true/false</returns>
        Task<bool> CheckExistAsync(T item, CancellationToken cancel = default);


        /// <summary>
        /// Проверка на существование сущности по Id 
        /// </summary>
        /// <param name="id">Id проверяемой сущности</param>
        /// <param name="cancel">Возможность отмены операции</param>
        /// <returns></returns>
        Task<bool> CheckExistByIdAsync(Guid id, CancellationToken cancel = default);
    }
}
