using LiteDB;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace MTGinator.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private string _databasePath;

        public PlayerRepository(IConfiguration config)
        {
            _databasePath = config["DatabasePath"];
        }

        public IEnumerable<Player> GetPlayers()
        {
            using var db = new LiteDatabase(_databasePath);

            var playerCollection = db.GetCollection<Player>("players");
            return playerCollection.FindAll();
        }

        public void SavePlayers(IEnumerable<Player> players)
        {
            using var db = new LiteDatabase(_databasePath);
            var playerCollection = db.GetCollection<Player>("players");

            //Finds the list of players that doesn't already exists and adds it
            foreach (var playerToAdd in players.Where(p => !playerCollection.Exists(q => q.Name == p.Name)))
            {
                playerCollection.Insert(playerToAdd);
            }
        }
    }
}
