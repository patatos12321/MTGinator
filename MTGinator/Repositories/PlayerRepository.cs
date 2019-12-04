using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public class PlayerRepository
    {
        public IEnumerable<MTGinator.Player> GetPlayers()
        {
            return new List<Player>();
        }
    }
}
