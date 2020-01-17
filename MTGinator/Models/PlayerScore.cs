namespace MTGinator.Models
{
    public class PlayerScore
    {
        public readonly Player Player;

        public int Score { get; set; }

        public PlayerScore(Player player)
        {
            Player = player;
        }
    }
}
