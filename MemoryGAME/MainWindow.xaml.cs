using System.Windows;
using MemoryGAME.Services;
using MemoryGAME.ViewModels;
using MemoryGAME.Views;

namespace MemoryGAME
{
    public partial class MainWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();


            var statisticsService = new StatisticsService();
            var imageService = new ImageService();
            var gameService = new GameSaveService(imageService, statisticsService);
            var userService = new UserService(gameService, statisticsService);


            _viewModel = new LoginViewModel(userService);
            DataContext = _viewModel;


            _viewModel.PlayCommand = new Utilities.RelayCommand(PlayGame, () => _viewModel.CanPlay);
        }

        private void PlayGame()
        {
            try
            {
                if (_viewModel.SelectedUser != null)
                {

                    var gameWindow = new MainGameWindow(_viewModel.SelectedUser.Username);
                    gameWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting game: {ex.Message}\n\n{ex.StackTrace}", "Error");
            }
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayGame();
        }
    }
}
