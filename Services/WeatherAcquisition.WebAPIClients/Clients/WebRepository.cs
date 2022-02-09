using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.WebAPIClients.Clients
{
    /// <summary>
    /// Репозиторий как Клиент
    /// </summary>
    public class WebRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly HttpClient _client;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="client">Http-клиент</param>
        public WebRepository(HttpClient client) => this._client = client;

        //#################################################################################################
        #region IRepository<T>

        public Task<T> AddAsync(T item, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckExistAsync(T item, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckExistByIdAsync(Guid id, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> DeleteAsync(T item, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> DeleteByIdAsync(Guid id, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(Guid Id, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsync(T item, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }


        #endregion // IRepository<T>
    }
}
