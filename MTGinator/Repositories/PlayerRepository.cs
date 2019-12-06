using LiteDB;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace MTGinator.Repositories
{
    public class PlayerRepository : AbstractRepository<Player>
    {
        public PlayerRepository(IConfiguration config) : base(config) { }

        protected override LiteCollection<Player> GetLiteCollection()
        {
            return _database.GetCollection<Player>(DbCollectionName.Player);
        }
    }
}
