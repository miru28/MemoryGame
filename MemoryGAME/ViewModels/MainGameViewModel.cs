using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MemoryGAME.Models;
using MemoryGAME.Services;

namespace MemoryGAME.ViewModels
{
    public class MainGameViewModel : INotifyPropertyChanged
    {
        private readonly GameSaveService _gameService;
        private readonly ImageService _imageService;
        private string _currentUsername;
        private GameState _currentGame;
        private Timer _gameTimer;
        private Card _firstSelectedCard;
        private Card _secondSelectedCard;
        private bool _isProcessingTurn;
        private bool _gameOver;
        private string _statusMessage;
        private int _timeRemaining;

        public ObservableCollection<Card> Cards { get; private set; }

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public ObservableCollection<string> Categories { get; private set; }

        private string _selectedCategory;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private int _timeLimit = 60;
        public int TimeLimit
        {
            get => _timeLimit;
            set
            {
                _timeLimit = value;
                OnPropertyChanged(nameof(TimeLimit));
            }
        }

        public int TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                _timeRemaining = value;
                OnPropertyChanged(nameof(TimeRemaining));
                OnPropertyChanged(nameof(TimeRemainingFormatted));
            }
        }

        public string TimeRemainingFormatted => $"{TimeRemaining / 60:D2}:{TimeRemaining % 60:D2}";

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public bool GameOver
        {
            get => _gameOver;
            set
            {
                _gameOver = value;
                OnPropertyChanged(nameof(GameOver));
            }
        }


        public ICommand NewGameCommand { get; private set; }
        public ICommand SaveGameCommand { get; private set; }
        public ICommand OpenGameCommand { get; private set; }
        public ICommand CardClickCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        public MainGameViewModel(GameSaveService gameService, ImageService imageService, string username)
        {

            _gameService = gameService;
            _imageService = imageService;
            _currentUsername = username;
            Card.SetImageService(imageService);


            Cards = new ObservableCollection<Card>();
            Categories = new ObservableCollection<string>(_imageService.GetCategories());

            if (Categories.Count > 0)
                SelectedCategory = Categories[0];


            NewGameCommand = new RelayCommand(StartNewGame);
            SaveGameCommand = new RelayCommand(SaveGame, CanSaveGame);
            OpenGameCommand = new RelayCommand(LoadGame);
            CardClickCommand = new RelayCommand<Card>(CardClick, CanClickCard);
            ExitCommand = new RelayCommand(ExitGame);

            StatusMessage = "Ready to play!";
            GameOver = false;

        }

        private bool CanClickCard(Card card)
        {
            return !_isProcessingTurn && !card.IsFlipped && !card.IsMatched && !GameOver;
        }

        private async void CardClick(Card card)
        {
            if (GameOver) return;


            card.IsFlipped = true;


            if (_firstSelectedCard == null)
            {
                _firstSelectedCard = card;
            }
            else if (_secondSelectedCard == null && _firstSelectedCard.Id != card.Id)
            {
                _secondSelectedCard = card;
                _isProcessingTurn = true;


                await Task.Delay(500);

                if (_firstSelectedCard.ImagePath == _secondSelectedCard.ImagePath)
                {

                    _firstSelectedCard.IsMatched = true;
                    _secondSelectedCard.IsMatched = true;
                    StatusMessage = "Match found!";


                    if (Cards.All(c => c.IsMatched))
                    {
                        GameWon();
                    }
                }
                else
                {

                    _firstSelectedCard.IsFlipped = false;
                    _secondSelectedCard.IsFlipped = false;
                    StatusMessage = "No match. Try again.";
                }


                _firstSelectedCard = null;
                _secondSelectedCard = null;
                _isProcessingTurn = false;
            }
        }

        private void StartNewGame()
        {
            try
            {

                string categoryToUse = "Test";


                if (SelectedCategory != null &&
                    SelectedCategory != "Test" &&
                    _imageService.GetCategories().Contains(SelectedCategory))
                {
                    categoryToUse = SelectedCategory;
                }


                _gameTimer?.Dispose();


                _currentGame = _gameService.CreateNewGame(categoryToUse, Rows, Columns, TimeLimit);


                Cards.Clear();
                foreach (var card in _currentGame.Cards)
                {
                    Cards.Add(card);
                }


                TimeRemaining = TimeLimit;
                _gameTimer = new Timer(TimerCallback, null, 0, 1000);

                StatusMessage = "Game started. Find all matches!";
                GameOver = false;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error starting game: {ex.Message}";
                MessageBox.Show($"Could not start game: {ex.Message}\n\nPlease make sure you have images in the category folders.",
                               "Error Starting Game",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }



        public void SetCustomDimensions(int rows, int columns)
        {
            if (rows * columns % 2 != 0)
            {
                StatusMessage = "The total number of cards must be even.";
                return;
            }

            Rows = rows;
            Columns = columns;
            OnPropertyChanged(nameof(Rows));
            OnPropertyChanged(nameof(Columns));
        }

        private void TimerCallback(object state)
        {

            App.Current.Dispatcher.Invoke(() =>
            {
                if (TimeRemaining > 0)
                {
                    TimeRemaining--;
                    _currentGame.TimeRemaining = TimeRemaining;
                    _currentGame.ElapsedTime = TimeLimit - TimeRemaining;
                }
                else if (!GameOver)
                {
                    GameLost();
                }
            });
        }

        private void GameWon()
        {
            _gameTimer?.Dispose();
            StatusMessage = "Congratulations! You won!";
            GameOver = true;
            _gameService.RecordGameResult(_currentUsername, true);
        }

        private void GameLost()
        {
            _gameTimer?.Dispose();
            StatusMessage = "Time's up! Game over.";
            GameOver = true;
            _gameService.RecordGameResult(_currentUsername, false);
        }

        private bool CanSaveGame()
        {
            return _currentGame != null && !GameOver;
        }

        private void SaveGame()
        {
            if (_currentGame != null)
            {
                _gameService.SaveGame(_currentGame, _currentUsername);
                StatusMessage = "Game saved successfully.";
            }
        }

        private void LoadGame()
        {

            _gameTimer?.Dispose();


            _currentGame = _gameService.LoadGame(_currentUsername);

            if (_currentGame == null)
            {
                StatusMessage = "No saved game found.";
                return;
            }


            Rows = _currentGame.Rows;
            Columns = _currentGame.Columns;
            SelectedCategory = _currentGame.Category;
            TimeRemaining = _currentGame.TimeRemaining;

            Cards.Clear();
            foreach (var card in _currentGame.Cards)
            {
                Cards.Add(card);
            }


            _gameTimer = new Timer(TimerCallback, null, 0, 1000);

            StatusMessage = "Game loaded successfully. Continue playing!";
            GameOver = false;
            OnPropertyChanged(nameof(Rows));
            OnPropertyChanged(nameof(Columns));
        }

        public void ExitGame()
        {
            _gameTimer?.Dispose();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return parameter == null || _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}