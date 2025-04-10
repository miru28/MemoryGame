using System.Windows;
using MemoryGAME.Services;


namespace MemoryGAME.Views
{
    public partial class StatisticsWindow : Window
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsWindow(StatisticsService statisticsService)
        {
            InitializeComponent();
            _statisticsService = statisticsService;


            StatisticsGrid.ItemsSource = _statisticsService.GetAllStatistics();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
