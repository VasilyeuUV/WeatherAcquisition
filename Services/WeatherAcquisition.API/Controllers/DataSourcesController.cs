using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        /// Получение полного количества записей
        /// </summary>
        /// <returns></returns>
        [HttpGet("count")] // - указывает, как этот метод будет выглядеть со стороны WebAPI (т.е. как обратиться к нему)
                           // типа api/count
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))] // - для документирования WebAPI
                                                                            // (статусный код, который возвращает метод; возвращаемый тип данных)
        public async Task<IActionResult> GetItemsCount()
            => Ok(await _repository.GetCountAsync());



    }
}
