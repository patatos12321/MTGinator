using Microsoft.Extensions.Configuration;
using MTGinator.Models;


namespace MTGinator.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(IConfiguration config) : base(config)
        {
            
        }

        public override Event GetById(int id)
        {
            return GetLiteCollection()
                .Include(x => x.ParticipatingPlayers)
                .Include(x => x.Rounds)
                .FindOne(d => d.Id == id);
        }
    }
}
