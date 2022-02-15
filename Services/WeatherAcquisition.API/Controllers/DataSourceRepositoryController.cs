using AutoMapper;
using WeatherAcquisition.API.Controllers._Base;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Domain.Base.Models;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.API.Controllers
{

    /// <summary>
    /// Контроллер для управления источниками данных 
    /// </summary>
    // адрес указан в базовом классе
    public class DataSourceRepositoryController : AMappedEntityController<DataSourceDomainBaseModel, DataSource>
    {

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public DataSourceRepositoryController(IRepository<DataSource> repository, IMapper mapper)
            : base(repository, mapper) { }
    }
}
