﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;
using WeatherAcquisition.Interfaces.Base.EntityAbilities;

namespace WeatherAcquisition.Interfaces.Base.Repositories
{
    /// <summary>
    /// Репозиторий для CRUD операций
    /// </summary>
    /// <typeparam name="T">Сущность</typeparam>
    public interface IRepositoryCRUDAsync<T> : IAddableAsync<T>, IReadableAsync<T>, IUpdatableAsync<T>, IDeletableAsync<T>
        where T : IEntity
    {

        /// <summary>
        /// Получить все сущности
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

    }
}
