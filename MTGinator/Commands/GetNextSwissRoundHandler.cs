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
    public class GetNextSwissRoundHandler : IRequestHandler<GetNextRandomRound, Round>
    {
        private readonly IEventRepository _eventRepository;

        private Event _event;

        public GetNextSwissRoundHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Task<Round> Handle(GetNextRandomRound request, CancellationToken cancellationToken)
        {
            _event = GetEvent(request);
            Round nextRound = GetNewRound(_event);

            var playerSwissPairingArray = _event.ParticipatingPlayers.Select(p => new PlayerSwissPairing(p, GetNbOfWins(@event, p))).ToArray();

            //First round is completely random
            if (nextRound.Number == 1)
            {
                //we have as many pairings as the number of players halved, rounded down
                return Task.FromResult(HandleFirstRoundRandomly(nextRound, playerSwissPairingArray));
            }

            while (!EveryPlayerIsPaired(nextRound, playerSwissPairingArray))
            {
                var player1 = GetUnpairedPlayerWithTheMostWin(nextRound, playerSwissPairingArray);

            }

            return Task.FromResult(nextRound);
        }

        private Round GetNewRound()
        {
            return new Round
            {
                Number = _event.Rounds.Count + 1,
                Pairings = new List<Pairing>()
            };
        }

        private Event GetEvent(GetNextRandomRound request)
        {
            var @event = _eventRepository.GetById(request.EventId);
            if (@event.Rounds == null)
            {
                @event.Rounds = new List<Round>();
            }

            return @event;
        }

        private int GetNbOfWins(Player player)
        {
            return _event.Rounds
                .SelectMany(r => r.Pairings)
                .Select(p => p.WinningPlayer)
                .Where(p => p.Id == player.Id).Count();
        }

        private static bool EveryPlayerIsPaired(Round nextRound, PlayerSwissPairing[] playerSwissPairingArray)
        {
            var pairedPlayers = nextRound.Pairings.Select(x => x.Players).SelectMany(p => p);
            foreach (var playerWin in playerSwissPairingArray)
            {
                if (!pairedPlayers.Any(p => p.Id == playerWin.Player.Id))
                {
                    return false;
                }
            }
            return true;
        }

        private static Round HandleFirstRoundRandomly(Round nextRound, PlayerSwissPairing[] playerSwissPairingArray)
        {
            var alreadyUsedNumbers = new HashSet<int>();
            var randomGenerator = new Random();

            for (int i = 0; i < playerSwissPairingArray.Length / 2; i++)
            {
                var player1 = GetUnpairedRandomPlayer(playerSwissPairingArray, alreadyUsedNumbers, randomGenerator);
                var player2 = GetUnpairedRandomPlayer(playerSwissPairingArray, alreadyUsedNumbers, randomGenerator);

                var pairing = new Pairing
                {
                    Players = new List<Player>()
                };
                pairing.Players.Add(player1.Player);
                pairing.Players.Add(player2.Player);
                nextRound.Pairings.Add(pairing);
            }

            return nextRound;
        }

        private static PlayerSwissPairing GetUnpairedRandomPlayer(PlayerSwissPairing[] playerSwissPairingArray, HashSet<int> alreadyUsedNumbers, Random randomGenerator)
        {
            var randomNumber = randomGenerator.Next(0, playerSwissPairingArray.Length);
            while (alreadyUsedNumbers.Contains(randomNumber))
            {
                randomNumber = randomGenerator.Next(0, playerSwissPairingArray.Length);
            }
            alreadyUsedNumbers.Add(randomNumber);
            return playerSwissPairingArray[randomNumber];
        }

        private static PlayerSwissPairing GetUnpairedPlayerWithTheMostWin(Round nextRound, PlayerSwissPairing[] playerSwissPairingArray)
        {
            var playersOrderedByNbWins = playerSwissPairingArray
                .Where(p => !p.IsPaired)
                .OrderByDescending(p => p.NbWins)
                .ToArray();

            if (playersOrderedByNbWins.Length == 0)
            {
                return null;
            }

            return playersOrderedByNbWins[0];
        }

        private PlayerSwissPairing GetNextOpposingPlayer(PlayerSwissPairing player, Round nextRound, PlayerSwissPairing[] playerSwissPairingArray)
        {
            var playersOrderedByNbWins = playerSwissPairingArray
                .Where(p => !p.IsPaired && p.Player.Id != player.Player.Id)
                .OrderByDescending(p => p.NbWins)
                .ToArray();


            if (playersOrderedByNbWins.Length == 0)
            {
                return null;
            }

            for (int i = 0; i < playersOrderedByNbWins.Length; i++)
            {
                if (!HavePlayedTogether(player, playersOrderedByNbWins[i]))
                {
                    return playersOrderedByNbWins[i];
                }
            }

            return null;
        }

        private bool HavePlayedTogether(PlayerSwissPairing player1, PlayerSwissPairing player2)
        {
            var pairingsWithPlayer1 = _event.Rounds
                .SelectMany(r => r.Pairings)
                .Where(pairing => pairing.Players.Any(player => player.Id == player1.Player.Id));

            if (pairingsWithPlayer1.Any(pairing => pairing.Players.Any(player => player.Id == player2.Player.Id)))
            {
                return true;
            }

            return false;
        }
    }
}
