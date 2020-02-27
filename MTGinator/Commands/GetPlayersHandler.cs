using MediatR;
using MTGinator.Models;
using MTGinator.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MTGinator.Commands
{
    public class GetPlayersHandler : IRequestHandler<GetPlayers, IEnumerable<Player>>
    {
        private readonly IRepository<Player> _playerRepository;
        private readonly IResultRepository _resultRepository;

        public GetPlayersHandler(IRepository<Player> playerRepository, IResultRepository resultRepository)
        {
            _playerRepository = playerRepository;
            _resultRepository = resultRepository;
        }

        public Task<IEnumerable<Player>> Handle(GetPlayers request, CancellationToken cancellationToken)
        {
            var players = _playerRepository.GetAll().ToList();

            foreach (var player in players)
            {
                var results = _resultRepository.GetByPlayerId(player.Id);
                player.Score = results.Sum(r => r.Score);
            }

            return Task.FromResult((IEnumerable<Player>)players.OrderByDescending(p => p.Score));
        }
    }
}
