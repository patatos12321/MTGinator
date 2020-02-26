using MTGinator.Models;

namespace MTGinator.Commands
{
    public class PlayerSwissPairing
    {
        public Player Player { get; }
        public int NbWins { get; }
        public bool IsPaired { get; set; }

        public PlayerSwissPairing(Player player, int nbWin)
        {
            Player = player;
            NbWins = nbWin;
        }
    }
}
