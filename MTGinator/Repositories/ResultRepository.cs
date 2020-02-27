using Microsoft.Extensions.Configuration;
using MTGinator.Models;
using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public class ResultRepository : GenericRepository<Result>, IResultRepository
    {
        public ResultRepository(IConfiguration config) : base(config)
        {
            
        }

        public IEnumerable<Result> GetByEventId(int id)
        {
            return GetLiteCollection()
                .Include(x => x.Player)
                .Include(x => x.Event)
                .Find(x => x.Event.Id == id);
        }

        public IEnumerable<Result> GetByPlayerId(int id)
        {
            return GetLiteCollection()
                .Find(x => x.Player.Id == id);
        }
    }
}
