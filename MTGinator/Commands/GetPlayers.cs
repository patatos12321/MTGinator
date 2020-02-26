using MediatR;
using MTGinator.Models;
using System.Collections.Generic;

namespace MTGinator.Commands
{
    public class GetPlayers : IRequest<IEnumerable<Player>>
    {
    }
}
