using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.EntityAbilities
{
    /// <summary>
    /// Сущность может быть изменена
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IUpdatableAsync<T> where T : IEntity
    {
        /// <summary>
        /// Обновление (изменение) сущности 
        /// </summary>
        /// <param name="item">Изменяемая сущность</param>
        /// <param name="cancel">Возможность прервать операцию</param>
        /// <returns>Измененную сущность или пустую ссылку</returns>
        Task<T> UpdateAsync(T item, CancellationToken cancel = default);
    }
}
