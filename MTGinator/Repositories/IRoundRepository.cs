using MTGinator.Models;
using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public interface IRoundRepository : IRepository<Round>
    {
        IEnumerable<Round> GetListByEventId(int eventId);
    }
}
