using System.IO;
using MemoryGAME.Models;
using Newtonsoft.Json;

namespace MemoryGAME.Services
{
    public class GameSaveService
    {
        private readonly ImageService _imageService;
        private readonly StatisticsService _statisticsService;
        private const string SavedGamesDirectory = "SavedGames";



        public GameSaveService(ImageService imageService, StatisticsService statisticsService)
        {
            _imageService = imageService;
            _statisticsService = statisticsService;


            if (!Directory.Exists(SavedGamesDirectory))
            {
                Directory.CreateDirectory(SavedGamesDirectory);
            }
        }



        public GameState CreateNewGame(string category, int rows, int columns, int timeLimit)
        {
            int cardCount = rows * columns;


            if (cardCount % 2 != 0)
            {
                throw new ArgumentException("The total number of cards must be even.");
            }

            var imagePaths = _imageService.GetRandomImagesForCategory(category, cardCount);
            var cards = new List<Card>();

            for (int i = 0; i < cardCount; i++)
            {
                cards.Add(new Card
                {
                    Id = i,
                    ImagePath = imagePaths[i],
                    IsFlipped = false,
                    IsMatched = false
                });
            }

            return new GameState
            {
                Cards = cards,
                Category = category,
                TimeRemaining = timeLimit,
                ElapsedTime = 0,
                Rows = rows,
                Columns = columns
            };
        }

        public void SaveGame(GameState gameState, string username)
        {
            string filePath = Path.Combine(SavedGamesDirectory, $"{username}.json");
            string json = JsonConvert.SerializeObject(gameState);
            File.WriteAllText(filePath, json);
        }

        public GameState LoadGame(string username)
        {
            string filePath = Path.Combine(SavedGamesDirectory, $"{username}.json");

            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GameState>(json);
        }

        public void DeleteSavedGame(string username)
        {
            string filePath = Path.Combine(SavedGamesDirectory, $"{username}.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void RecordGameResult(string username, bool won)
        {
            _statisticsService.UpdateStatistics(username, won);
        }
    }
}