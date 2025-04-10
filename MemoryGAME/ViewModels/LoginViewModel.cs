using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using MemoryGAME.Models;
using MemoryGAME.Services;
using Microsoft.Win32;

namespace MemoryGAME.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly UserService _userService;
        private User _selectedUser;
        private string _newUsername;
        private string _newUserImagePath;

        public ObservableCollection<User> Users { get; private set; }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                OnPropertyChanged(nameof(CanPlay));
                OnPropertyChanged(nameof(CanDelete));
            }
        }

        public string NewUsername
        {
            get => _newUsername;
            set
            {
                _newUsername = value;
                OnPropertyChanged(nameof(NewUsername));
                OnPropertyChanged(nameof(CanCreateUser));
            }
        }

        public string NewUserImagePath
        {
            get => _newUserImagePath;
            set
            {
                _newUserImagePath = value;
                OnPropertyChanged(nameof(NewUserImagePath));
                OnPropertyChanged(nameof(CanCreateUser));
            }
        }

        public bool CanPlay => SelectedUser != null;
        public bool CanDelete => SelectedUser != null;
        public bool CanCreateUser => !string.IsNullOrWhiteSpace(NewUsername) && !string.IsNullOrEmpty(NewUserImagePath);


        public ICommand CreateUserCommand { get; private set; }
        public ICommand BrowseImageCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand PlayCommand { get; set; }

        public LoginViewModel(UserService userService)
        {
            _userService = userService;


            LoadUsers();


            CreateUserCommand = new MemoryGAME.Utilities.RelayCommand(CreateUser, () => CanCreateUser);
            BrowseImageCommand = new MemoryGAME.Utilities.RelayCommand(BrowseImage);
            DeleteUserCommand = new MemoryGAME.Utilities.RelayCommand(DeleteUser, () => CanDelete);
            PlayCommand = new MemoryGAME.Utilities.RelayCommand(Play, () => CanPlay);

        }

        private void LoadUsers()
        {
            var usersList = _userService.GetAllUsers();
            Users = new ObservableCollection<User>(usersList);
        }

        private void CreateUser()
        {
            try
            {

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string relativeImagePath = MakeRelativePath(baseDir, NewUserImagePath);

                var newUser = new User
                {
                    Username = NewUsername,
                    ImagePath = relativeImagePath
                };

                _userService.AddUser(newUser);


                Users.Add(newUser);


                SelectedUser = newUser;


                NewUsername = string.Empty;
                NewUserImagePath = string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
            }
        }

        private void BrowseImage()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif",
                Title = "Select User Image"
            };

            if (dialog.ShowDialog() == true)
            {
                NewUserImagePath = dialog.FileName;
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {

                var userToDelete = SelectedUser;


                SelectedUser = null;


                _userService.DeleteUser(userToDelete.Username);


                Users.Remove(userToDelete);
            }
        }

        private void Play()
        {

        }


        private string MakeRelativePath(string fromPath, string toPath)
        {
            if (string.IsNullOrEmpty(fromPath)) throw new ArgumentNullException(nameof(fromPath));
            if (string.IsNullOrEmpty(toPath)) throw new ArgumentNullException(nameof(toPath));

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme)
            {
                return toPath;
            }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}