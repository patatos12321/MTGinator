using MediatR;
using MTGinator.Models;
using MTGinator.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MTGinator.Commands
{
    public class GetEventResultsHandler : IRequestHandler<GetEventResults, IEnumerable<Result>>
    {
        private int[] ScoreByPlace = { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };

        private readonly IEventRepository _eventRepository;
        private readonly IResultRepository _resultRepository;

        public GetEventResultsHandler(IEventRepository eventRepository, IResultRepository resultRepository)
        {
            _eventRepository = eventRepository;
            _resultRepository = resultRepository;
        }

        public Task<IEnumerable<Result>> Handle(GetEventResults request, CancellationToken cancellationToken)
        {
            var existingResults = _resultRepository.GetByEventId(request.EventId);
            if (existingResults != null && existingResults.Count() >= 1)
            {
                return Task.FromResult(existingResults);
            }

            //if the results don't exist yet, create them
            var @event = _eventRepository.GetById(request.EventId);

            var results = new List<Result>();

            foreach (var participatingPlayer in @event.ParticipatingPlayers)
            {
                var result = new Result
                {
                    Player = participatingPlayer,
                    Event = @event,
                    NbWin = GetPlayerScore(@event, participatingPlayer.Id)
                };

                results.Add(result);
            }

            var resultsOrderedByNbWin = results.OrderByDescending(r => r.NbWin).ToArray();
            var finalOrderedList = new List<Result>();

            bool skipSecond = false;
            bool foundFirstPlaceMatchup = false;
            int lastNbWin = 0;
            int lastPlace = 0;

            for (int i = 0; i < resultsOrderedByNbWin.Count(); i++)
            {
                var currentResult = resultsOrderedByNbWin[i];
                if (i == 0 && resultsOrderedByNbWin[1].NbWin == currentResult.NbWin)
                {
                    var nextResult = resultsOrderedByNbWin[1];

                    foreach (var round in @event.Rounds)
                    {
                        foreach (var pairing in round.Pairings)
                        {
                            if (pairing.Players.Where(p => p.Id == currentResult.Player.Id).Count() == 1
                                && pairing.Players.Where(p => p.Id == nextResult.Player.Id).Count() == 1)
                            {
                                if (pairing.WinningPlayer.Id == currentResult.Player.Id)
                                {
                                    //First player won
                                    currentResult.Place = 1;
                                    currentResult.Score = ScoreByPlace[0];

                                    finalOrderedList.Add(currentResult);
                                }
                                else
                                {
                                    //Second player won
                                    skipSecond = true;
                                    nextResult.Place = 1;
                                    nextResult.Score = ScoreByPlace[0];
                                    finalOrderedList.Add(nextResult);

                                    currentResult.Place = 2;
                                    currentResult.Score = ScoreByPlace[1];
                                    finalOrderedList.Add(currentResult);
                                }

                                foundFirstPlaceMatchup = true;
                            }
                        }
                    }

                    if (!foundFirstPlaceMatchup)
                    {
                        //First player won and never fought against second place
                        currentResult.Place = 1;
                        currentResult.Score = ScoreByPlace[0];

                        finalOrderedList.Add(currentResult);
                    }

                    continue;
                }
                else if(i == 1)
                {
                    lastNbWin = currentResult.NbWin;
                    lastPlace = 2;
                    if (skipSecond)
                    {
                        continue;
                    }
                    currentResult.Place = 2;
                    currentResult.Score = ScoreByPlace[1];
                }
                else
                {
                    if (currentResult.NbWin == lastNbWin)
                    {
                        currentResult.Place = lastPlace;
                        currentResult.Score = ScoreByPlace[lastPlace - 1];
                    }
                    else
                    {
                        currentResult.Place = finalOrderedList.Count() + 1;
                        currentResult.Score = ScoreByPlace[currentResult.Place - 1];
                        lastNbWin = currentResult.NbWin;
                        lastPlace = currentResult.Place;
                    }
                }

                finalOrderedList.Add(currentResult);
            }

            _resultRepository.Save(finalOrderedList);

            @event.IsFinished = true;
            _eventRepository.Save(@event);

            return Task.FromResult((IEnumerable<Result>)finalOrderedList);
        }

        private static int GetPlayerScore(Event @event, int playerId)
        {
            int nbWin = 0;
            foreach (var round in @event.Rounds)
            {
                foreach (var pairing in round.Pairings)
                {
                    if (pairing.WinningPlayer.Id == playerId)
                    {
                        nbWin++;
                    }
                }
            }

            return nbWin;
        }
    }
}
