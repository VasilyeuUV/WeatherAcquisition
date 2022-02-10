using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.WebAPIClients.Clients
{
    /// <summary>
    /// Клиент, который будет общаться с контроллером
    /// (здесь он прикидывается репозиторием. т.е. имитирует работу сервиса репозитория)
    /// </summary>
    public class WebRepositoryClient<T> : IRepository<T> where T : IEntity
    {
        private readonly HttpClient _client;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="client">Http-клиент</param>
        public WebRepositoryClient(HttpClient client) => this._client = client;

        //#################################################################################################
        #region IRepository<T>

        public async Task<T> AddAsync(T item, CancellationToken cancel = default)
        {
            var response = await _client                // - отправляем запрос
                .PostAsJsonAsync("", item, cancel)
                .ConfigureAwait(false);
            var result = await response
                .EnsureSuccessStatusCode()              // - проверка статусного кода
                .Content
                .ReadFromJsonAsync<T>(cancellationToken: cancel)
                .ConfigureAwait(false);
            return result;
        }



        public async Task<bool> CheckExistAsync(T item, CancellationToken cancel = default)
        {
            var response = await _client
                .PostAsJsonAsync("exist", item, cancel)         // - отправляем запрос на конечную точку "exist"
                .ConfigureAwait(false);

            return response.StatusCode != System.Net.HttpStatusCode.NotFound    // - если статусный код не равен 404 (NotFound)
                && response.IsSuccessStatusCode;                                // и при этом успешный (один из 200-тых)
                                                                                // то объект с запрошенным Id существует
        }



        public async Task<bool> CheckExistByIdAsync(Guid id, CancellationToken cancel = default)
        {
            var response = await _client
                .GetAsync($"exist/id/{id}", cancel)     // get-запрос с указанием конечной точки
                .ConfigureAwait(false);

            return response.StatusCode != System.Net.HttpStatusCode.NotFound    // - если статусный код не равен 404 (NotFound)
                && response.IsSuccessStatusCode;                                // и при этом успешный (один из 200-тых)
                                                                                // то объект с запрошенным Id существует
        }



        public async Task<T> DeleteAsync(T item, CancellationToken cancel = default)
        {
            // Вручную сформируем запрос типа Delete, заставим сериализоваться объект

            // - формируем запрос на Удаление по адресу "пустая строка" (сам контроллер)
            var request = new HttpRequestMessage(HttpMethod.Delete, "")
            {
                Content = JsonContent.Create(item)  // - контент запроса, в который создаём Json-контент
            };

            // - выполняем запрос
            var response = await _client
                .SendAsync(request, cancel)
                .ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return default;

            var result = await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>(cancellationToken: cancel)
                .ConfigureAwait(false);
            return result;
        }


        public async Task<T> DeleteByIdAsync(Guid id, CancellationToken cancel = default)
        {
            var response = await _client
                .DeleteAsync($"{id}", cancel)
                .ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return default;

            var result = await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadFromJsonAsync<T>(cancellationToken: cancel)
                .ConfigureAwait(false);
            return result;
        }


        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default)
            => await _client
            .GetFromJsonAsync<IEnumerable<T>>("", cancel)
            .ConfigureAwait(false);



        public async Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default)
            => await _client
            .GetFromJsonAsync<IEnumerable<T>>($"items[{skip},{count}]", cancel)
            .ConfigureAwait(false);



        public async Task<T> GetByIdAsync(Guid Id, CancellationToken cancel = default)
            => await _client
            .GetFromJsonAsync<T>($"{Id}",cancel)
            .ConfigureAwait(false);



        public async Task<long> GetCountAsync(CancellationToken cancel = default)
            => await _client
            .GetFromJsonAsync<long>("count", cancel)     // - метод будет автоматически серилизовать JSON
            .ConfigureAwait(false);



        public async Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            var response = await _client
                .GetAsync($"page/{pageIndex}/{pageSize}", cancel)
                .ConfigureAwait(false);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new PageItemModel
                {
                    Items = Enumerable.Empty<T>(),
                    ItemsCount = 0,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                };
            return await response
               .EnsureSuccessStatusCode()      // - убеждаемся, что статусный код корректен
               .Content
               .ReadFromJsonAsync<PageItemModel>(cancellationToken: cancel)
               .ConfigureAwait(false);                                          
        }

        /// <summary>
        /// Модель страницы
        /// </summary>
        private class PageItemModel : IPage<T>
        {
            //------------------------------------------------------------------
            #region IPage<T>

            public int PageIndex { get; init; }
            public int PageSize { get; init; }
            public IEnumerable<T> Items { get; init; }
            public long ItemsCount { get; init; }

            #endregion // IPage<T>
        }



        public async Task<T> UpdateAsync(T item, CancellationToken cancel = default)
        {
            var response = await _client                // - отправляем запрос
                .PutAsJsonAsync("", item, cancel)
                .ConfigureAwait(false);
            var result = await response
                .EnsureSuccessStatusCode()              // - проверка статусного кода
                .Content
                .ReadFromJsonAsync<T>(cancellationToken: cancel)
                .ConfigureAwait(false);
            return result;
        }

        #endregion // IRepository<T>
    }
}
