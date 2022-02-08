using WeatherAcquisition.DAL.Contexts;
using WeatherAcquisition.DAL.Entities._Base;

namespace WeatherAcquisition.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с именованными сущностями
    /// </summary>
    public class DbNamedRepository<T> : DbRepository<T>
        where T : AEntity, new()
    {

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="dbContext">Контекст БД</param>
        public DbNamedRepository(DataBaseContext dbContext) : base(dbContext) { }


    }
}
