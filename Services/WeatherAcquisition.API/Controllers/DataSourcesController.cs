using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAcquisition.DAL.Entities;
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
        /// Проверка на существование источника по егно Id
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
        [HttpGet("items[[{skip:int}:{count:int}]]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DataSource>>> Get(int skip, int count)
            => Ok(await _repository.GetAsync(skip, count));

    }
}
