using System;

namespace WeatherAcquisition.Interfaces.Base.Entities
{
    /// <summary>
    ///  Сущность. Определяется идентификатором
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        Guid Id { get; }
    }
}
