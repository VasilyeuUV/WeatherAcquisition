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
        public DbSet<DataValue> Values { get; set; }

        /// <summary>
        /// Источники данных
        /// </summary>
        public DbSet<DataSource> Sources { get; set; }


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
                .WithOne(v => v.Source)             // с отношением один ко многим
                .OnDelete(DeleteBehavior.Cascade);  // и политикой удаления (удаляем DataSource, удаляются все его DataValue)

            // Настройка уникального индекса для поля Name таблицы DataSource
            // Вариант 2 (первый вариант - в сущностях:
            //    [Index(nameof(Name), IsUnique = true)]       // создать индекс по колонке Name, чтобы названия были уникальными
            //    public class DataSource...
            modelBuilder.Entity<DataSource>()
                .HasIndex(source => source.Name)
                .IsUnique(true);

            // Индекс по времени при получении DataValue
            modelBuilder.Entity<DataValue>()
                .HasIndex(source => source.GetedValueDtg);

            //// поле Name таблицы DataSource обязательно для заполнения
            //modelBuilder.Entity<DataSource>()
            //    .Property(source => source.Name)
            //    .IsRequired();


        }
    }
}
