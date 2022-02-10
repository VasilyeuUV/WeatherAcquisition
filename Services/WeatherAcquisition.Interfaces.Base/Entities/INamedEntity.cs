namespace WeatherAcquisition.Interfaces.Base.Entities
{
    /// <summary>
    /// Именованная сущность
    /// </summary>
    public interface INamedEntity : IEntity
    {

        /// <summary>
        /// Наименование сущности
        /// </summary>
        string Name { get;}
    }
}
