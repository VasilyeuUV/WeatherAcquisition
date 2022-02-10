using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.DAL.Entities._Base
{
    /// <summary>
    /// Сущность
    /// </summary>
    public abstract class AEntity : IEntity
    {
        [Key]                                                   // поле будет ключевым
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   // автогенерация Guid
        [Required]
        public Guid Id { get; set; }
    }
}
