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
            return GetPlayerCollection().FindAll();
        }

        public void SavePlayers(IEnumerable<Player> players)
        {
            var playerCollection = GetPlayerCollection();

            //Finds the list of players that doesn't already exists and adds it
            foreach (var playerToAdd in players.Where(p => !playerCollection.Exists(q => q.Name == p.Name)))
            {
                playerCollection.Insert(playerToAdd);
            }
        }

        public void DeletePlayer(int id)
        {
            GetPlayerCollection().Delete(p => p.Id == id);
        }

        public void EditPlayer(Player player)
        {
            var playerCollection = GetPlayerCollection();
            var dbPlayer = playerCollection.FindOne(p => p.Id == player.Id);
            dbPlayer.Name = player.Name;
            playerCollection.Update(dbPlayer);
        }

        private LiteCollection<Player> GetPlayerCollection()
        {
            var db = new LiteDatabase(_databasePath);
            return db.GetCollection<Player>("players");
        }
    }
}
