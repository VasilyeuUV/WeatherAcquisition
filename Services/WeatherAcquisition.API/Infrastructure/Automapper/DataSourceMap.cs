using AutoMapper;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Domain.Base.Models;

namespace WeatherAcquisition.API.Infrastructure.Automapper
{
    public class DataSourceMap : Profile
    {
        public DataSourceMap()
        {
            // Правила проекции
            CreateMap<DataSourceDomainBaseModel, DataSource>()      // из DataSource в DataSourceDomainBaseModel
                .ReverseMap();                                      // обратное преобразование
        }
    }
}
