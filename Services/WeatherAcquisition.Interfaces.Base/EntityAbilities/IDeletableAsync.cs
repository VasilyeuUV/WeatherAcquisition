using System;
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
        /// Удаление сущности по ее Id
        /// </summary>
        /// <param name="id">Удаляемая сущность</param>
        /// <returns></returns>
        Task<T> DeleteByIdAsync(Guid id);
    }
}
