using WeatherAcquisition.Interfaces.Base.Entities;
using WeatherAcquisition.Interfaces.Base.Operations;

namespace WeatherAcquisition.Interfaces.Base.Repositories
{
    /// <summary>
    /// Репозиторий для текущего решения
    /// </summary>
    public interface IRepository<T> : IRepositoryCRUDAsync<T>, IRepositoryByPageAsync<T>, IGetCountAsync, ICheckExistAsync
        where T : IEntity
    {
    }
}
