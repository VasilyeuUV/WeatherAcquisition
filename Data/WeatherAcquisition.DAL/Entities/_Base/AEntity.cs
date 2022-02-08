using System;
using System.ComponentModel.DataAnnotations;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.DAL.Entities._Base
{
    /// <summary>
    /// Сущность
    /// </summary>
    public abstract class AEntity : IEntity
    {
        [Required]
        public Guid Id { get; set; }
    }
}
