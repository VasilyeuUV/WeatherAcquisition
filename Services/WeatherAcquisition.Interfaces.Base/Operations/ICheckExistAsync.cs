using System;
using System.Threading.Tasks;

namespace WeatherAcquisition.Interfaces.Base.Operations
{
    /// <summary>
    /// Проверить на существование сущности
    /// </summary>
    public interface ICheckExistAsync
    {
        /// <summary>
        /// Добавление сущности 
        /// </summary>
        /// <param name="item">Добавляемая сущность</param>
        /// <returns></returns>
        Task<bool> CheckExistAsync(Guid id);
    }
}
