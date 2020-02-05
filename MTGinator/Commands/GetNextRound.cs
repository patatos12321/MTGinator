using MediatR;
using MTGinator.Models;

namespace MTGinator.Commands
{
    public class GetNextRound : IRequest<Round>
    {
        public int EventId { get; }

        public GetNextRound(int eventId)
        {
            EventId = eventId;
        }
    }
}
