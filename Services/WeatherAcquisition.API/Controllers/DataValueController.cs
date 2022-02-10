using Microsoft.AspNetCore.Mvc;
using WeatherAcquisition.API.Controllers._Base;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.API.Controllers
{
    /// <summary>
    /// Контроллер, который будет управлять DataValue
    /// </summary>
    [ApiController]
    public class DataValueController : AEntityControllerBase<DataValue>
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="repository"></param>
        public DataValueController(IRepository<DataValue> repository) : base(repository) { }

    }
}
