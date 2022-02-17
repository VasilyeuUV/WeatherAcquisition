using System;
using System.ComponentModel.DataAnnotations;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Domain.Base.Models
{
    /// <summary>
    /// Доменная модель источников данных
    /// </summary>
    public class DataSourceDomainBaseModel : INamedEntity
    {
        /// <summary>
        /// Описание модели
        /// </summary>
        public string Description { get; set; }

        //########################################################################
        #region INamedEntity

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        #endregion // INamedEntity

    }
}
