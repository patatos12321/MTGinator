using Microsoft.Extensions.Configuration;
using MTGinator.Models;
using System.Collections.Generic;

namespace MTGinator.Repositories
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(IConfiguration config) : base(config)
        {
            
        }

        public override Event GetById(int id)
        {
            var @event = GetLiteCollection()
                .Include(x => x.ParticipatingPlayers)
                .FindOne(d => d.Id == id);

            var playerCollection = GetLiteCollectionForType<Player>();
            if (@event.Rounds == null)
            {
                @event.Rounds = new List<Round>();
            }

            //Fetch the players by hand because the "Include" syntax doesn't allow multiple layers of lists
            foreach (var round in @event.Rounds)
            {
                foreach (var pairing in round.Pairings)
                {
                    for (int playerIndex = 0; playerIndex < pairing.Players.Count; playerIndex++)
                    {
                        pairing.Players[playerIndex] = playerCollection.FindById(pairing.Players[playerIndex].Id);
                    }
                    pairing.WinningPlayer = playerCollection.FindById(pairing.WinningPlayer.Id);
                }
            }

            return @event;
        }
    }
}
