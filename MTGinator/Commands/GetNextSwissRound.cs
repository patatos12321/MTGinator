using MediatR;
using MTGinator.Models;

namespace MTGinator.Commands
{
    public class GetNextSwissRound : IRequest<Round>
    {
        public int EventId { get; }

        public GetNextSwissRound(int eventId)
        {
            EventId = eventId;
        }
    }
}
