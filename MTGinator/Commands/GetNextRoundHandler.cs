using MediatR;
using MTGinator.Models;
using MTGinator.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MTGinator.Commands
{
    public class GetNextRoundHandler : IRequestHandler<GetNextRound, Round>
    {
        private readonly IEventRepository _eventRepository;

        public GetNextRoundHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Task<Round> Handle(GetNextRound request, CancellationToken cancellationToken)
        {
            var @event = _eventRepository.GetById(request.EventId);
            if (@event.Rounds == null)
            {
                @event.Rounds = new List<Round>();
            }

            var nextRound = new Round
            {
                Number = @event.Rounds.Count + 1,
                Pairings = new List<Pairing>()
            };

            var playerArray = @event.ParticipatingPlayers.ToArray();

            //First round is completely random
            if (nextRound.Number == 1)
            {
                //we have as many pairings as the number of players halved, rounded down
                HandleFirstRound(nextRound, playerArray);
                return Task.FromResult(nextRound);
            }

            for (int i = 0; i < playerArray.Length / 2; i++)
            {
                bool isOnlyUniquePairings = true;
                do
                {
                    nextRound.Pairings = new List<Pairing>();
                    HandleFirstRound(nextRound, playerArray);
                    isOnlyUniquePairings = true;

                    foreach (var round in @event.Rounds)
                    {
                        foreach (var pairing in round.Pairings)
                        {
                            foreach (var nextPairing in nextRound.Pairings)
                            {
                                if ((nextPairing.Players[0].Id == pairing.Players[0].Id || nextPairing.Players[0].Id == pairing.Players[1].Id)
                                    && (nextPairing.Players[1].Id == pairing.Players[0].Id || nextPairing.Players[1].Id == pairing.Players[1].Id))
                                {
                                    isOnlyUniquePairings = false;
                                }
                            }
                        }
                    }

                } while (!isOnlyUniquePairings);
            }

            return Task.FromResult(nextRound);
        }

        private static void HandleFirstRound(Round nextRound, Player[] playerArray)
        {
            var alreadyUsedNumbers = new HashSet<int>();
            var randomGenerator = new Random();

            for (int i = 0; i < playerArray.Length / 2; i++)
            {
                var player1 = GetRandomPlayerThatHasntBeenUsedBefore(playerArray, alreadyUsedNumbers, randomGenerator);
                var player2 = GetRandomPlayerThatHasntBeenUsedBefore(playerArray, alreadyUsedNumbers, randomGenerator);

                var pairing = new Pairing
                {
                    Players = new List<Player>()
                };
                pairing.Players.Add(player1);
                pairing.Players.Add(player2);
                nextRound.Pairings.Add(pairing);
            }
        }

        private static Player GetRandomPlayerThatHasntBeenUsedBefore(Player[] playerArray, HashSet<int> alreadyUsedNumbers, Random randomGenerator)
        {
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
