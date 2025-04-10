using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using MemoryGAME.Services;
using MemoryGAME.ViewModels;


namespace MemoryGAME.Views
{
    public partial class MainGameWindow : Window
    {
        private readonly MainGameViewModel _viewModel;
        private readonly StatisticsService _statisticsService;

        public MainGameWindow(string username)
        {
            InitializeComponent();


            var imageService = new ImageService();
            _statisticsService = new StatisticsService();
            var gameService = new GameSaveService(imageService, _statisticsService);


            _viewModel = new MainGameViewModel(gameService, imageService, username);
            DataContext = _viewModel;
        }

        private void StandardGame_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetCustomDimensions(4, 4);
        }

        private void CustomGame_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CustomGameDialog();
            if (dialog.ShowDialog() == true)
            {
                _viewModel.SetCustomDimensions(dialog.Rows, dialog.Columns);
            }
        }

        private void ApplyTimeLimit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {

            var aboutDialog = new Window
            {
                Title = "About Memory Game",
                Width = 350,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.ToolWindow
            };


            var stackPanel = new StackPanel
            {
                Margin = new Thickness(20),
            };


            stackPanel.Children.Add(new TextBlock
            {
                Text = "Memory Game",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = "Developed by Miruna Lupu",
                Margin = new Thickness(0, 0, 0, 5)
            });


            var emailTextBlock = new TextBlock
            {
                Margin = new Thickness(0, 0, 0, 5)
            };

            emailTextBlock.Inlines.Add(new Run("Email: "));
            var hyperlink = new Hyperlink(new Run("cristina.lupu@student.unitbv.ro"));
            hyperlink.NavigateUri = new Uri("https://student.unitbv.ro/surgeweb");
            hyperlink.RequestNavigate += (s, args) =>
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = args.Uri.AbsoluteUri,
                    UseShellExecute = true
                });
                args.Handled = true;
            };
            emailTextBlock.Inlines.Add(hyperlink);
            stackPanel.Children.Add(emailTextBlock);

            stackPanel.Children.Add(new TextBlock
            {
                Text = "Group: 10LF332",
                Margin = new Thickness(0, 0, 0, 5)
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = "IA",
                Margin = new Thickness(0, 0, 0, 15)
            });


            var okButton = new Button
            {
                Content = "OK",
                Width = 80,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            okButton.Click += (s, args) => aboutDialog.Close();
            stackPanel.Children.Add(okButton);

            aboutDialog.Content = stackPanel;
            aboutDialog.ShowDialog();
        }


        private void StatisticsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var statisticsWindow = new StatisticsWindow(_statisticsService);
            statisticsWindow.ShowDialog();
        }
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {

            _viewModel.ExitGame();


            var loginWindow = new MainWindow();
            loginWindow.Show();


            this.Close();
        }
    }
}