using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAcquisition.Interfaces.Base.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.API.Controllers._Base
{
    /// <summary>
    /// Тип контроллера, выполняющий проекцию сущности T из базовой сущности TBase, получаемой из БД 
    /// </summary>
    /// <typeparam name="T">Проекция новой сущности</typeparam>
    /// <typeparam name="TBase">Базовая (исходная) сущность</typeparam>
    [ApiController, Route("api/[controller]")]
    public abstract class AMappedEntityController<T, TBase> : ControllerBase
        where T : IEntity
        where TBase : IEntity
    {
        private readonly IRepository<TBase> _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="repository">Репозиторий</param>
        /// <param name="mapper">Автомаппер</param>
        protected AMappedEntityController(IRepository<TBase> repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }



        //#################################################################################################################
        #region Repository actions

        /// <summary>
        /// Получение источников данных, хранящихся в БД
        /// </summary>
        /// <returns>Количество источников данных</returns>
        [HttpGet("count")] // - указывает, как этот метод будет выглядеть со стороны WebAPI (т.е. как обратиться к нему)
                           // типа api/count
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))] // - для документирования WebAPI
                                                                             // (статусный код, который возвращает метод; возвращаемый тип данных)
        public async Task<IActionResult> GetItemsCountAsync()
            => Ok(await _repository.GetCountAsync());



        /// <summary>
        /// Проверка на существование источника по его Id
        /// </summary>
        /// <param name="id">Id источника данных</param>
        /// <returns>true/false</returns>
        [HttpGet("exist/id/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> CheckExistIdAsync(Guid id)
            => await _repository.CheckExistByIdAsync(id)
            ? Ok(true)
            : NotFound(false);



        /// <summary>
        /// Проверка на существование источника данных
        /// </summary>
        /// <param name="item">Источник данных</param>
        /// <returns>true/false</returns>
        //[HttpGet("exist")]  // - Get запрос (не используем, ибо не правильно)
        [HttpPost("exist")] // - Post запрос
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> CheckExistAsync(T item)
            => await _repository.CheckExistAsync(Cast(item))
            ? Ok(true)
            : NotFound(false);



        /// <summary>
        /// Извлечение всех данных репозитория (таблицы данных)
        /// </summary>
        /// <returns></returns>
        [HttpGet]   // - выгрузим всё содержимое 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => Ok(Cast(await _repository.GetAllAsync()));



        /// <summary>
        /// Получение источников данных по частям
        /// </summary>
        /// <param name="skip">Количество пропущенных источников данных</param>
        /// <param name="count">Количество получаемых источников данных</param>
        /// <returns>Список источников данных</returns>
        [HttpGet("items[[{skip:int},{count:int}]]")]           // - двойные квадратные скобки для экранирования
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<T>>> Get(int skip, int count)
            => Ok(Cast(await _repository.GetAsync(skip, count)));



        /// <summary>
        /// Получить источник данных по Id
        /// </summary>
        /// <param name="id">Id источника данных</param>
        /// <returns>Источник данных или null</returns>
        [HttpGet("{id:Guid}")]
        [ActionName("Get")]     // - название метода
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
            => (await _repository.GetByIdAsync(id) is { } item)
                ? Ok(item)
                : NotFound();



        /// <summary>
        /// Запись страницы с данными
        /// </summary>
        /// <param name="Items"></param>
        /// <param name="ItemsCount"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        protected record Page(IEnumerable<T> Items, long ItemsCount, int PageIndex, int PageSize) : IPage<T>
        {
            public int PagesCount => PageSize > 0
                ? (int)Math.Ceiling((double)ItemsCount / PageSize)
                : 0;
        }

        /// <summary>
        /// Получение страницы с источниками данных
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns>Страница с источниками данных</returns>
        [HttpGet("page/{pageIndex:int}/{pageSize:int}")]       // - вариант вызова 1
        [HttpGet("page[[{pageIndex:int},{pageSize:int}]]")]    // - вариант вызова 2
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<T>>> GetPage(int pageIndex, int pageSize)
        {
            var result = await _repository.GetPageAsync(pageIndex, pageSize);
            return result.Items.Any()
                ? Ok(Cast(result))
                : NotFound(result);
        }



        /// <summary>
        /// Добавление источника данных
        /// </summary>
        /// <param name="item">Добавляемый источник данных</param>
        /// <returns>Добавленный источник данных</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]    // - там будет адрес, по которому можно будет
                                                                // обратиться для получения созданного элемента
        public async Task<IActionResult> Add(T item)
        {
            var result = await _repository.AddAsync(Cast(item));                          // - репозиторий возвращает объект, который добавил
            return CreatedAtAction(nameof(Get), new { id = result.Id }, Cast(result));    // - запрашиваем в репозитории новый источник по id
        }



        /// <summary>
        /// Обновление существующего источника данных
        /// </summary>
        /// <param name="item">Источник данных с новыми значениями</param>
        /// <returns>Источник данных с обновлёнными значениями или null</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(T item)
        {
            if (await _repository.UpdateAsync(Cast(item)) is not { } result)
                return NotFound(item);
            return AcceptedAtAction(nameof(Get), new { id = result.Id }, Cast(result));
        }



        /// <summary>
        /// Удаление источника данных
        /// </summary>
        /// <param name="item">Удаляемый источник данных</param>
        /// <returns>Удалённый источник данных</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(T item)
        {
            if (await _repository.DeleteAsync(Cast(item)) is not { } result)
                return NotFound(item);
            return Ok(Cast(result));
        }



        /// <summary>
        /// Удаление источника данных по Id
        /// </summary>
        /// <param name="id">Id удаляемого источника данных</param>
        /// <returns>Удалённый источник данных</returns>
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            if (await _repository.DeleteByIdAsync(id) is not { } result)
                return NotFound(id);
            return Ok(Cast(result));
        }

        #endregion // Repository actions




        //#################################################################################################################
        #region ПРОЕКЦИИ

        /// <summary>
        /// Проекция из доменной модели в базовую (обратная проекция)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual TBase Cast(T item) => _mapper.Map<TBase>(item);

        /// <summary>
        /// Проекция из базовой модели в доменную (прямая проекция)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual T Cast(TBase item) => _mapper.Map<T>(item);


        /// <summary>
        /// Проекция из списка доменных моделей в список базовых моделей (обратная проекция)
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TBase> Cast(IEnumerable<T> items) => _mapper.Map<IEnumerable<TBase>>(items);

        /// <summary>
        /// Проекция из списка базовых моделей в список доменных моделей (прямая проекция)
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual IEnumerable<T> Cast(IEnumerable<TBase> items) => _mapper.Map<IEnumerable<T>>(items);


        protected virtual IPage<T> Cast(IPage<TBase> page)
            => new Page(
                Cast(page.Items), 
                page.ItemsCount, 
                page.PageIndex, 
                page.PageSize
                );

        #endregion // ПРОЕКЦИИ

    }
}
