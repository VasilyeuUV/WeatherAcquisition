using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherAcquisition.DAL.Contexts;
using WeatherAcquisition.DAL.Entities._Base;
using WeatherAcquisition.Interfaces.Base.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;

namespace WeatherAcquisition.DAL.Repositories
{
    /// <summary>
    /// Репозиторий базы данных, который будет работать только с конкретными сущностями из БД<br/>
    /// Будет частью сервиса приложения и храниться в контейнере сервисов
    /// </summary>
    public class DbRepository<T> : IRepository<T> where T : AEntity, new()
    {
        private readonly DataBaseContext _dbContext;    // контекст базы данных

        /// <summary>
        /// Набор данных (типа таблица с данными)
        /// </summary>
        protected DbSet<T> Set { get; }

        /// <summary>
        /// Возможность расширения.<br/>
        /// Позволяет наложить доп. ограничения 
        /// типа Set.Where(...), Set.Include(...) и т.п.
        /// которые сузят объём выборки из ДБ
        /// </summary>
        protected virtual IQueryable<T> Items => Set;

        /// <summary>
        /// Запись, обозначающая страницу данных и реализует интефейс IPage<T>
        /// </summary>
        /// <param name="Index">Номер страницы</param>
        /// <param name="Size">Количество элементов, помещающихся на странице (размер страницы)</param>
        /// <param name="Items">Список элементов страницы</param>
        /// <param name="ItemsTotalCount">Общее количество элементов, возможных для получения</param>
        protected record Page(int PageIndex, int PageSize, IEnumerable<T> Items, long ItemsCount) : IPage<T>;


        /// <summary>
        /// Разрешает или запрещает автоматическое сохранение изменений в БД
        /// </summary>
        public bool AutoSaveChanges { get; set; }


        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
        public DbRepository(DataBaseContext dbContext)
        {
            this._dbContext = dbContext;
            Set = this._dbContext.Set<T>();
        }


        /// <summary>
        /// Сохранение в БД
        /// </summary>
        /// <param name="cancel">Возможность прервать операцию</param>
        /// <returns>Чесло, указывающее количество реально выполненных изменений</returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancel = default)
            => await _dbContext
            .SaveChangesAsync(cancel)
            .ConfigureAwait(false);

        //########################################################################################################################
        #region IRepository<T>

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancel = default)
            => await Items
            .ToArrayAsync(cancel)
            .ConfigureAwait(false);



        public async Task<T> GetByIdAsync(Guid Id, CancellationToken cancel = default)
        {
            // Учитываем, что DbSet кэширует полученные с БД данные
            return Items switch
            {
                // проверяем наличие в кэше
                DbSet<T> set => await set
                                        .FindAsync(new object[] { Id }, cancel)
                                        .ConfigureAwait(false),
                // { } === IQueryable<T>
                { } items => await items
                                        .SingleOrDefaultAsync(i => i.Id == Id, cancel)  // объект в БД может быть только один, иначе - исключение
                                                                                        // (не учитывает кэш, который формирует dbContext)
                                        .ConfigureAwait(false),
                _ => throw new InvalidOperationException("Ошибка в определении источника данных"),
            };
        }



        public async Task<IEnumerable<T>> GetAsync(int skip, int count, CancellationToken cancel = default)
        {
            if (count <= 0)
                return Enumerable.Empty<T>();

            IQueryable<T> query = Items switch
            {
                IOrderedQueryable<T> orderedQuery => orderedQuery,  // - если Items - упорядоченная последовательность, то её и возвращаем
                { } q => q.OrderBy(i => i.Id)                       // - если нет - упорядочиваем.
            };
            if (skip > 0)
                query = query.Skip(skip); ;

            return await query
                .Take(count)
                .ToArrayAsync(cancel)
                .ConfigureAwait(false);
        }



        public async Task<IPage<T>> GetPageAsync(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            var defaultPage = new Page(
                    Items: Enumerable.Empty<T>(),
                    PageIndex: pageIndex,
                    PageSize: pageSize,
                    ItemsCount: 0
                    //ItemsCount: await GetCountAsync(cancel).ConfigureAwait(false)
                    );

            if (pageSize <= 0)
                return defaultPage;

            // Запрос 
            var query = Items;
            var itemsCount = await query
                .CountAsync(cancel)
                .ConfigureAwait(false);
            if (itemsCount == 0)
                return defaultPage;

            if (query is not IOrderedQueryable<T>)
                query = query.OrderBy(i => i.Id);

            if (pageIndex > 0)
                query = query.Skip(pageIndex * pageSize);   // пропускаем

            query = query.Take(pageSize);                   // забираем

            // получаем сами элементы (выполняем запрос)
            var items = await query
                .ToArrayAsync(cancel)
                .ConfigureAwait(false);

            return new Page(
                Items: items,
                PageIndex: pageIndex,
                PageSize: pageSize,
                ItemsCount: itemsCount);
        }



        public async Task<long> GetCountAsync(CancellationToken cancel = default)
            => await Items
            .CountAsync(cancel)
            .ConfigureAwait(false);



        public async Task<T> AddAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            // Следующие три записи одинаковы по результату
            //_dbContext.Entry(item).State = EntityState.Added;
            //Set.Add(item);
            await _dbContext
                .AddAsync(item, cancel)
                .ConfigureAwait(false);

            if (AutoSaveChanges)
                await SaveChangesAsync(cancel).ConfigureAwait(false);

            //await _dbContext
            //    .SaveChangesAsync(cancel)           // каждая операция будет сохраняться в БД
            //    .ConfigureAwait(false);

            return item;
        }


        public async Task<T> UpdateAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            // Следующие три записи одинаковы по результату
            //_dbContext.Entry(item).State = EntityState.Modified;
            //Set.Update(item);
            _dbContext.Update(item);

            if (AutoSaveChanges)
                await SaveChangesAsync(cancel).ConfigureAwait(false);

            //await _dbContext
            //    .SaveChangesAsync(cancel)            // каждая операция будет сохраняться в БД
            //    .ConfigureAwait(false);

            return item;

        }


        public async Task<T> DeleteAsync(T item, CancellationToken cancel = default)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (!await CheckExistByIdAsync(item.Id, cancel))
                return null;

            // Следующие три записи одинаковы по результату
            //_dbContext.Entry(item).State = EntityState.Deleted;
            //Set.Remove(item);
            _dbContext.Remove(item);

            if (AutoSaveChanges)
                await SaveChangesAsync(cancel).ConfigureAwait(false);

            //await _dbContext
            //    .SaveChangesAsync(cancel)           // каждая операция будет сохраняться в БД
            //    .ConfigureAwait(false);

            return item;
        }


        public async Task<T> DeleteByIdAsync(Guid id, CancellationToken cancel = default)
        {
            // Учитываем возможность нахождения в кэше
            // Local - это кэшированные объекты)
            var item = Set.Local.FirstOrDefault(i => i.Id == id);
            if (item is null)
                item = await Set                                        // пытаемся получить из БД
                    .Select(i => new T { Id = i.Id})
                    .FirstOrDefaultAsync(i => i.Id == id, cancel)
                    .ConfigureAwait(false);
            return item is null 
                ? null 
                : await DeleteAsync(item, cancel).ConfigureAwait(false);
        }


        public async Task<bool> CheckExistAsync(T item, CancellationToken cancel = default)
            => item is null
            ? throw new ArgumentNullException(nameof(item))
            : await Items
                .AnyAsync(i => i.Id == item.Id, cancel)
                .ConfigureAwait(false);



        public async Task<bool> CheckExistByIdAsync(Guid id, CancellationToken cancel = default)
            => await Items
                .AnyAsync(i => i.Id == id, cancel)
                .ConfigureAwait(false);             // - не захватывать контекст синхронизации (исключает возможность мёртвых блокировок)
                                                    // (false ВСЕГДА!!! для библиотек, не связанных с интерфейсом пользователя
                                                    // и логикой обработки контекста hеtp-запроса web-приложения)

        #endregion // IRepository<T>


    }
}
