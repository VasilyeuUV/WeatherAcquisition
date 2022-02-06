using System;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        Guid Id { get; }
    }
}
