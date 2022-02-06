using Microsoft.EntityFrameworkCore;

namespace WeatherAcquisition.DAL.Contexts
{
    internal class DataBaseContext : DbContext
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="options"></param>
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
    }
}
