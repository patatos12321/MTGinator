using MediatR;
using MTGinator.Models;
using MTGinator.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MTGinator.Commands
{
    public class GetNextRoundHandler : IRequestHandler<GetNextRound, Round>
    {
        private readonly IRoundRepository _roundRepository;
        private readonly IRepository<Event> _eventRepository;

        public GetNextRoundHandler(IRoundRepository roundRepository, IRepository<Event> eventRepository)
        {
            _roundRepository = roundRepository;
            _eventRepository = eventRepository;
        }

        public Task<Round> Handle(GetNextRound request, CancellationToken cancellationToken)
        {
            var rounds = _roundRepository.GetListByEventId(request.EventId).ToList();
            var @event = _eventRepository.GetById(request.EventId);

            var nextRound = new Round
            {
                Number = rounds.Count + 1
            };

            var playerArray = @event.ParticipatingPlayers.ToArray();
            var alreadyUsedNumbers = new HashSet<int>();
            var randomGenerator = new Random();

            //First round is completely random
            if (nextRound.Number == 1)
            {
                //we have as many pairings as the number of players halved, rounded down
                for (int i = 0; i < playerArray.Length / 2; i++)
                {
                    var player1 = GetRandomPlayerThatHasntBeenUsedBefore(playerArray, alreadyUsedNumbers, randomGenerator);
                    var player2 = GetRandomPlayerThatHasntBeenUsedBefore(playerArray, alreadyUsedNumbers, randomGenerator);

                    var pairing = new Pairing();
                    pairing.Players.Add(player1);
                    pairing.Players.Add(player2);
                    nextRound.Pairings.Add(pairing);
                }
                return Task.FromResult(nextRound);
            }

            return Task.FromResult(new Round());
        }

        private static Player GetRandomPlayerThatHasntBeenUsedBefore(Player[] playerArray, HashSet<int> alreadyUsedNumbers, Random randomGenerator)
        {
            var pairing = new Pairing();
            var randomNumber = randomGenerator.Next(0, playerArray.Length);
            while (alreadyUsedNumbers.Contains(randomNumber))
            {
                randomNumber = randomGenerator.Next(0, playerArray.Length);
            }
            alreadyUsedNumbers.Add(randomNumber);
            return playerArray[randomNumber];
        }
    }
}
