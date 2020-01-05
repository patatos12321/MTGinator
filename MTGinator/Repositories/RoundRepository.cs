using Microsoft.Extensions.Configuration;
using MTGinator.Models;
using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public class RoundRepository : GenericRepository<Round>, IRoundRepository
    {
        public RoundRepository(IConfiguration config) : base(config)
        {
            
        }

        public IEnumerable<Round> GetListByEventId(int eventId)
        {
            return GetLiteCollection().Find(p => p.Event.Id == eventId);
        }
    }
}
