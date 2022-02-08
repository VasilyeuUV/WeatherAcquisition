using System.ComponentModel.DataAnnotations;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.DAL.Entities._Base
{
    /// <summary>
    /// Именованная сущность
    /// </summary>
    public abstract class ANamedEntity : AEntity, INamedEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
