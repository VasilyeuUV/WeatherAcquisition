using System;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.EntityAbilities
{
    /// <summary>
    /// Сущность может быть прочитана (получена) по Id
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IReadableAsync<T> where T : IEntity
    {
        /// <summary>
        /// Чтение (получение) сущности по ее Id
        /// </summary>
        /// <param name="Id">Id получаемой сущности</param>
        /// <returns></returns>
        Task<T> GetByIdAsync(Guid Id);
    }
}
