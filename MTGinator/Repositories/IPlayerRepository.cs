using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public interface IPlayerRepository
    {
        IEnumerable<Player> GetPlayers();

        void SavePlayers(IEnumerable<Player> players);
    }
}
