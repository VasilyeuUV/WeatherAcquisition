using Microsoft.EntityFrameworkCore;
using WeatherAcquisition.DAL.Entities;

namespace WeatherAcquisition.DAL.Contexts
{
    /// <summary>
    /// Контекст базы данных
    /// </summary>
    public class DataBaseContext : DbContext
    {
        /// <summary>
        /// Значения
        /// </summary>
        DbSet<DataValue> Values { get; set; }

        /// <summary>
        /// Источники данных
        /// </summary>
        DbSet<DataSource> Sources { get; set; }



        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="options"></param>
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }


        /// <summary>
        /// Настройка модели
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataSource>()       // имеем этот тип в БД
                .HasMany<DataValue>()               // который имеет множество этих значений 
                .WithOne(v => v.Source)             // с о тношением один ко многим
                .OnDelete(DeleteBehavior.Cascade);  // и политикой удаления (удаляем DataSource, удаляются все его DataValue)
        }
    }
}
