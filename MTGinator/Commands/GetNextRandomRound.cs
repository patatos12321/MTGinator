using MediatR;
using MTGinator.Models;

namespace MTGinator.Commands
{
    public class GetNextRandomRound : IRequest<Round>
    {
        public int EventId { get; }

        public GetNextRandomRound(int eventId)
        {
            EventId = eventId;
        }
    }
}
