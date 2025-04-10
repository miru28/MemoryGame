namespace MemoryGAME.Models
{
    public class GameState
    {
        public List<Card> Cards { get; set; }
        public string Category { get; set; }
        public int TimeRemaining { get; set; }
        public int ElapsedTime { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
}