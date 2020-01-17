using MediatR;
using MTGinator.Models;
using System.Collections.Generic;

namespace MTGinator.Commands
{
    public class GetEventResults : IRequest<IEnumerable<Result>>
    {
        public int EventId { get; }

        public GetEventResults(int eventId)
        {
            EventId = eventId;
        }
    }
}
