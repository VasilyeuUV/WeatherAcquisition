using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WeatherAcquisition.DAL.Contexts;
using WeatherAcquisition.DAL.Entities;

namespace WeatherAcquisition.API.Data
{

    /// <summary>
    /// Инициализатор базы данных. Будет сервисом
    /// </summary>
    public class DataBaseContextInitializer
    {
        private readonly DataBaseContext _dbContext;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="dbContext">Контекст базы данных</param>
        public DataBaseContextInitializer(DataBaseContext dbContext) 
            => this._dbContext = dbContext;

        /// <summary>
        /// Инициализация базы данных некоторыми произвольными данными
        /// </summary>
        public void Initialize()
        {
            // Применяем все миграции
            // и создаём базу данных, если она ещё не была создана
            _dbContext.Database.Migrate();

            // Если в БД есть хоть один источник,
            // то инициализация не требуется
            if (_dbContext.Sources.Any())
                return;

            // Создаём источники

            var rnd = new Random();
            for (int i = 1; i <= 10; i++)
            {
                var source = new DataSource
                {
                    Id = Guid.NewGuid(),
                    Name = $"Источник_{i}",
                    Description = $"Тестовый источник данных №{i}"
                };
                _dbContext.Sources.Add(source);

                var values = new DataValue[rnd.Next(10, 20)];
                // - c кортежем
                for (var (j, count) = (0, values.Length); j < count; j++)
                {
                    var value = new DataValue
                    {
                        Id = Guid.NewGuid(),
                        Source = source,
                        GetedValueDtg = DateTimeOffset.Now.AddDays(-rnd.Next(0, 365)),
                        Value = $"{rnd.Next(0, 30)}"
                    };
                    //_dbContext.Values.Add(value);
                    values[j] = value;
                }
                _dbContext.Values.AddRange(values);
            }
            _dbContext.SaveChanges();
        }
    }
}
