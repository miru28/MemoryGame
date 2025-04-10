using System.IO;
using MemoryGAME.Models;
using Newtonsoft.Json;


namespace MemoryGAME.Services
{
    public class StatisticsService
    {
        private const string StatisticsFilePath = "statistics.json";

        public List<GameStatistics> GetAllStatistics()
        {
            if (!File.Exists(StatisticsFilePath))
                return new List<GameStatistics>();

            var json = File.ReadAllText(StatisticsFilePath);
            return JsonConvert.DeserializeObject<List<GameStatistics>>(json) ?? new List<GameStatistics>();
        }

        public GameStatistics GetUserStatistics(string username)
        {
            var statistics = GetAllStatistics();
            return statistics.FirstOrDefault(s => s.Username == username);
        }

        public void UpdateStatistics(string username, bool won)
        {
            var statistics = GetAllStatistics();
            var userStats = statistics.FirstOrDefault(s => s.Username == username);

            if (userStats == null)
            {
                userStats = new GameStatistics { Username = username };
                statistics.Add(userStats);
            }

            userStats.GamesPlayed++;
            if (won) userStats.GamesWon++;

            SaveStatistics(statistics);
        }

        public void DeleteUserStatistics(string username)
        {
            var statistics = GetAllStatistics();
            statistics.RemoveAll(s => s.Username == username);
            SaveStatistics(statistics);
        }

        private void SaveStatistics(List<GameStatistics> statistics)
        {
            var json = JsonConvert.SerializeObject(statistics);
            File.WriteAllText(StatisticsFilePath, json);
        }
    }
}