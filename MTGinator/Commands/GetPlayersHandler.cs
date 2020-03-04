using System;
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
        private readonly IEventRepository _eventRepository;

        public GetPlayersHandler(IRepository<Player> playerRepository, IResultRepository resultRepository, IEventRepository eventRepository)
        {
            _playerRepository = playerRepository;
            _resultRepository = resultRepository;
            _eventRepository = eventRepository;
        }

        public Task<IEnumerable<Player>> Handle(GetPlayers request, CancellationToken cancellationToken)
        {
            var players = _playerRepository.GetAll().ToList();
            Dictionary<int, Event> eventsById = new Dictionary<int, Event>();

            foreach (var player in players)
            {
                decimal nbLoss = 0;
                decimal nbWin = 0;
                var results = _resultRepository.GetByPlayerId(player.Id).ToList();
                player.Score = results.Sum(r => r.Score);

                foreach (var result in results)
                {
                    var eventId = result.Event.Id;
                    Event @event;

                    if (eventsById.ContainsKey(eventId))
                    {
                        @event = eventsById[eventId];
                    }
                    else
                    {
                        @event = _eventRepository.GetById(eventId);
                        eventsById.Add(eventId, @event);
                    }

                    foreach (var pairing in @event.Rounds.SelectMany(round => round.Pairings)
                        .Where(pairing => pairing.Players.Exists(pairingPlayer => pairingPlayer.Id == player.Id)))
                    {
                        if (pairing.WinningPlayer.Id == player.Id)
                        {
                            nbWin++;
                        }

                        nbLoss++;
                    }
                }

                if (nbWin == 0)
                {
                    player.WinLossRatio = 0;
                }
                else if (nbLoss == 0)
                {
                    player.WinLossRatio = 100;
                }
                else
                {
                    player.WinLossRatio = Math.Round(nbWin / nbLoss * 100, 2);
                }
            }

            return Task.FromResult((IEnumerable<Player>)players.OrderByDescending(p => p.Score));
        }
    }
}
