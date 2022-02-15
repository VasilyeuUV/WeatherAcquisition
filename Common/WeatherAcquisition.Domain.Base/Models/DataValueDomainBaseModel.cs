using System;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Domain.Base.Models
{
    /// <summary>
    /// Доменная модель данных
    /// </summary>
    public class DataValueDomainBaseModel : IEntity
    {

        public DateTimeOffset Dtg { get; set; }
        public string Value { get; set; }
        public bool IsFault { get; set; }

        //##############################################################################
        #region IEntity

        public Guid Id { get; }

        #endregion // IEntity

    }
}
