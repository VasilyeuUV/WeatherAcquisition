using System;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.DAL.Entities._Base
{
    /// <summary>
    /// Сущность
    /// </summary>
    public abstract class AEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}
