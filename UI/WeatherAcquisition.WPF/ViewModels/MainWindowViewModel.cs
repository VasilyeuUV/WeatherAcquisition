using System.Collections.ObjectModel;
using System.Windows.Input;
using WeatherAcquisition.DAL.Entities;
using WeatherAcquisition.Interfaces.Base.Repositories;
using WpfMvvmBase.Commands;
using WpfMvvmBase.ViewModels._Base;

namespace WeatherAcquisition.WPF.ViewModels
{

    /// <summary>
    /// Модель представления главного окна
    /// </summary>
    public class MainWindowViewModel : ATitledViewModel
    {
        private readonly IRepository<DataSource> _dataSourse;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="dataSourse">репозиторий источника данных</param>
        public MainWindowViewModel(IRepository<DataSource> dataSourse)
        {
            this._dataSourse = dataSourse;
        }

        //####################################################################################################
        #region PROPERTIES

        /// <summary>
        /// Источники данных
        /// </summary>
        public ObservableCollection<DataSource> DataSources { get; } = new();

        #endregion // PROPERTIES


        //####################################################################################################
        #region COMMANDS

        private ICommand? _loadDataSourceCommand = null;


        /// <summary>
        /// Команда для загрузки данных
        /// </summary>
        public ICommand LoadDataSourceCommand =>
            _loadDataSourceCommand ??= new LambdaCommand(async obj =>
            {
                DataSources.Clear();
                foreach (var source in await _dataSourse.GetAllAsync())
                    DataSources.Add(source);
            });


        #endregion // COMMANDS


    }
}
