using System.Collections.Generic;

namespace WeatherAcquisition.Interfaces.Base.Entities
{
    /// <summary>
    /// Страница
    /// </summary>
    /// <typeparam name="T">"Элемент страницы</typeparam>
    public interface IPage<out T>
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Количество элементов, помещающихся на странице (размер страницы)
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Список элементов страницы
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        /// Общее количество элементов, возможных для получения
        /// </summary>
        long ItemsCount { get; }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        int PagesCount { get; }
    }
}
