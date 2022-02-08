using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.DAL.Contexts;
using WeatherAcquisition.DAL.Entities._Base;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с именованными сущностями
    /// </summary>
    public class DbNamedRepository<T> : DbRepository<T>, INamedRepository<T>
        where T : ANamedEntity, new()
    {

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="dbContext">Контекст БД</param>
        public DbNamedRepository(DataBaseContext dbContext) : base(dbContext) { }



        //###################################################################################################
        #region INamedRepository

        public async Task<bool> CheckNameExistAsync(string name, CancellationToken cancel = default)
            => await Items
            .AnyAsync(i => i.Name == name, cancel)
            .ConfigureAwait(false);

        public async Task<T> DeleteByNameAsync(string name, CancellationToken cancel = default)
            => await Items
            .FirstOrDefaultAsync(i => i.Name == name, cancel)
            .ConfigureAwait(false);

        public async Task<T> GetByNameAsync(string name, CancellationToken cancel = default)
        {
            // Учитываем возможность нахождения в кэше
            // Local - это кэшированные объекты)
            var item = Set.Local.FirstOrDefault(i => i.Name == name);
            if (item is null)
                item = await Set                                        // пытаемся получить из БД
                    .Select(i => new T { 
                        Id = i.Id,
                        Name = i.Name 
                    })
                    .FirstOrDefaultAsync(i => i.Name == name, cancel)
                    .ConfigureAwait(false);
            return item is null
                ? null
                : await DeleteAsync(item, cancel).ConfigureAwait(false);
        }


        #endregion // INamedRepository

    }
}
