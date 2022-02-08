using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;

namespace WeatherAcquisition.Interfaces.Base.Repositories
{
    /// <summary>
    /// Репозиторий для именованных сущностей текущего решения
    /// </summary>
    public interface INamedRepository<T> : IRepository<T>
        where T : INamedEntity
    {
        /// <summary>
        /// Проверка на существование имени у сущности 
        /// </summary>
        /// <param name="item">Сущность</param>
        /// <param name="cancel">Возможность прервать операцию</param>
        /// <returns>Возвращает true/false</returns>
        Task<bool> CheckNameExistAsync(string name, CancellationToken cancel = default);

        /// <summary>
        /// Получение сущностей по имени
        /// </summary>
        /// <param name="name">Имя сущности</param>
        /// <param name="cancel">Возможность прервать операцию</param>
        /// <returns>Список сущностей с указанным именем</returns>
        Task<T> GetByNameAsync(string name, CancellationToken cancel = default);

        /// <summary>
        /// Удаление сущностей по имени
        /// </summary>
        /// <param name="name">Имя сущности</param>
        /// <param name="cancel">Возможность прервать операцию</param>
        /// <returns>Список удалённых по имени сущностей</returns>
        Task<T> DeleteByNameAsync(string name, CancellationToken cancel = default);

        ///// <summary>
        ///// Изменение имени сущности 
        ///// </summary>
        ///// <param name="item">Сущность с именем</param>
        ///// <param name="name">новое имя</param>
        ///// <param name="cancel">Возможность прервать операцию</param>
        ///// <returns>Сущность с новым именем</returns>
        //Task<T> UpdateNameAsync(T item, string name, CancellationToken cancel = default);
    }
}
