using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.EntityAbilities
{
    /// <summary>
    /// Сущность может быть добавлена
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IAddableAsync<T> where T : IEntity
    {
        /// <summary>
        /// Добавление сущности 
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        /// <returns></returns>
        Task<T> AddAsync(T item);
    }
}
