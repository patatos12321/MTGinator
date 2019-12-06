using LiteDB;
using Microsoft.Extensions.Configuration;

namespace MTGinator.Repositories
{
    public class EventRepository : AbstractRepository<Event>
    {

        public EventRepository(IConfiguration config) : base(config){}

        protected override LiteCollection<Event> GetLiteCollection()
        {
            return _database.GetCollection<Event>(DbCollectionName.Event);
        }
    }
}
