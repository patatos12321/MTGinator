using MTGinator.Models;
using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public interface IResultRepository : IRepository<Result>
    {
        IEnumerable<Result> GetByEventId(int id);
        IEnumerable<Result> GetByPlayerId(int id);
    }
}
