using System;
using WeatherAcquisition.DAL.Entities._Base;

namespace WeatherAcquisition.DAL.Entities
{
    /// <summary>
    /// Получаемые данные (сущность БД)
    /// </summary>
    public class DataValue : AEntity
    {

        /// <summary>
        /// Время получения значение в формате UTC (относительно нудевого меридиана)<br/>
        /// Осуществляются автоматические преобразования в конкретную временную зону
        /// </summary>
        public DateTimeOffset DtgGetValue { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Значение, которое получаем
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Источник, откуда получено значение
        /// </summary>
        public DataSource Source { get; set; }

        /// <summary>
        /// Флаг ошибки получения данных по любой причине
        /// </summary>
        public bool IsFault { get; set; } = false;
    }
}
