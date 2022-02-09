using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.API.Controllers
{
    /// <summary>
    /// Контроллер, который будет управлять DataSource
    /// </summary>
    [Route("api/[controller]")] // - адрес по умолчанию
    [ApiController]
    public class DataSourcesController : ControllerBase
    {
        private readonly IRepository<DataSource> _repository;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="repository">Репозиторий, к которому будет осуществлять доступ контроллер</param>
        public DataSourcesController(IRepository<DataSource> repository)
            => this._repository = repository;


        //#################################################################################################################
        #region Repository actions

        /// <summary>
        /// Получение источников данных, хранящихся в БД
        /// </summary>
        /// <returns>Количество источников данных</returns>
        [HttpGet("count")] // - указывает, как этот метод будет выглядеть со стороны WebAPI (т.е. как обратиться к нему)
                           // типа api/count
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))] // - для документирования WebAPI
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
        [HttpGet("exist")]  // - Get запрос
        [HttpPost("exist")] // - Post запрос
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(bool))]
        public async Task<IActionResult> CheckExistAsync(DataSource item)
            => await _repository.CheckExistAsync(item)
            ? Ok(true)
            : NotFound(false);



        /// <summary>
        /// Извлечение всех данных репозитория (таблицы данных)
        /// </summary>
        /// <returns></returns>
        [HttpGet]   // - выгрузим всё содержимое 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
            => Ok(await _repository.GetAllAsync());



        /// <summary>
        /// Получение источников данных по частям
        /// </summary>
        /// <param name="skip">Количество пропущенных источников данных</param>
        /// <param name="count">Количество получаемых источников данных</param>
        /// <returns>Список источников данных</returns>
        [HttpGet("items[[{skip:int},{count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DataSource>>> Get(int skip, int count)
            => Ok(await _repository.GetAsync(skip, count));



        /// <summary>
        /// Получить источник данных по Id
        /// </summary>
        /// <param name="id">Id источника данных</param>
        /// <returns>Источник данных или null</returns>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
            => (await _repository.GetByIdAsync(id) is { } item)
                ? Ok(item)
                : NotFound();



        /// <summary>
        /// Получение страницы с источниками данных
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns>Страница с источниками данных</returns>
        [HttpGet("page/{pageIndex:int}/{pageSize}")]           // - вариант вызова 1
        [HttpGet("page[[{pageIndex:int},{pageSize:int}]]")]    // - вариант вызова 2
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IPage<DataSource>>> GetPage(int pageIndex, int pageSize)
        {
            var result = await _repository.GetPageAsync(pageIndex, pageSize);
            return result.Items.Any()
                ? Ok(result)
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
        public async Task<IActionResult> Add(DataSource item)
        {
            var result = await _repository.AddAsync(item);                  // - репозиторий возвращает объект, который добавил
            return CreatedAtAction(nameof(GetById), new { id = result.Id });  // - запрашиваем в репозитории новый источник по id
        }



        /// <summary>
        /// Обновление существующего источника данных
        /// </summary>
        /// <param name="item">Источник данных с новыми значениями</param>
        /// <returns>Источник данных с обновлёнными значениями или null</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(DataSource item)
        {
            if (await _repository.UpdateAsync(item) is not { } result)
                return NotFound(item);
            return AcceptedAtAction(nameof(GetById), new { id = result.Id });
        }



        /// <summary>
        /// Удаление источника данных
        /// </summary>
        /// <param name="item">Удаляемый источник данных</param>
        /// <returns>Удалённый источник данных</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(DataSource item)
        {
            if (await _repository.DeleteAsync(item) is not { } result)
                return NotFound(item);
            return Ok(result);
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
            return Ok(result);
        }

        #endregion // Repository actions

    }
}
