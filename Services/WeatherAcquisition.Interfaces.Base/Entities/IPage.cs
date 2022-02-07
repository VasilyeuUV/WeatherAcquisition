using System;
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
        int Index { get; }

        /// <summary>
        /// Количество элементов, помещающихся на странице (размер страницы)
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Список элементов страницы
        /// </summary>
        public IEnumerable<T> Items { get; }

        /// <summary>
        /// Общее количество элементов, возможных для получения
        /// </summary>
        long ItemsTotalCount { get; }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        int PagesTotalCount => Size > 0 
            ? (int)Math.Ceiling((double)ItemsTotalCount / Size)
            : 0;



    }
}
