using System.ComponentModel.DataAnnotations;

namespace WeatherAcquisition.Interfaces.Base.Entities
{
    /// <summary>
    /// Именованная сущность
    /// </summary>
    public interface INamedEntity : IEntity
    {

        /// <summary>
        /// Наименование сущности
        /// </summary>
        [Required]
        string Name { get;}
    }
}
