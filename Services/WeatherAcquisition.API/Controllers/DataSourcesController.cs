using Microsoft.AspNetCore.Mvc;
using WeatherAcquisition.API.Controllers._Base;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.API.Controllers
{
    /// <summary>
    /// Контроллер, который будет управлять DataSource
    /// </summary>
    [ApiController]
    public class DataSourcesController : AEntityControllerBase<DataSource>
    {

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="repository"></param>
        public DataSourcesController(IRepository<DataSource> repository) : base(repository) { }

    }
}
