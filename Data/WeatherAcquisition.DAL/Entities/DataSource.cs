using System.Collections.Generic;
using WeatherAcquisition.DAL.Entities._Base;

namespace WeatherAcquisition.DAL.Entities
{
    /// <summary>
    /// Источник данных как именованная сущность
    /// </summary>
    public class DataSource : ANamedEntity
    {
        /// <summary>
        /// Описание источника данных
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Список значений<br/>
        /// Реализация отношения один-ко-многим.<br/>
        /// Можно не указывать, чтобы при запросе из источника данных автоматически не вытягивать все значения<br/> 
        /// Значения желательноизвлекать вручную, чтобы случайно не вытянуть все или не нужные<br/>
        /// Поэтому связь не указывается!!!
        /// </summary>
        //public ICollection<DataValue> Values { get; set; } = new HashSet<DataValue>();
    }
}
