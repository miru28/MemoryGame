using System.IO;
using MemoryGAME.Models;
using Newtonsoft.Json;

namespace MemoryGAME.Services
{
    public class UserService
    {
        private const string UsersFilePath = "users.json";
        private readonly GameSaveService _gameService;
        private readonly StatisticsService _statisticsService;

        public UserService(GameSaveService gameService, StatisticsService statisticsService)
        {
            _gameService = gameService;
            _statisticsService = statisticsService;
        }

        public List<User> GetAllUsers()
        {
            if (!File.Exists(UsersFilePath))
                return new List<User>();

            var json = File.ReadAllText(UsersFilePath);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }

        public User GetUser(string username)
        {
            return GetAllUsers().FirstOrDefault(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            var users = GetAllUsers();

            // Check if user already exists
            if (users.Any(u => u.Username == user.Username))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            users.Add(user);
            SaveUsers(users);
        }

        public void DeleteUser(string username)
        {
            var users = GetAllUsers();
            users.RemoveAll(u => u.Username == username);
            SaveUsers(users);

            // Delete user's saved game
            _gameService.DeleteSavedGame(username);

            // Delete user's statistics
            _statisticsService.DeleteUserStatistics(username);
        }

        private void SaveUsers(List<User> users)
        {
            var json = JsonConvert.SerializeObject(users);
            File.WriteAllText(UsersFilePath, json);
        }
    }
}