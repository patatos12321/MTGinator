using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTGinator.Repositories;

namespace MTGinator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(ILogger<WeatherForecastController> logger, IPlayerRepository playerRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public IEnumerable<Player> Get()
        {
            return _playerRepository.GetPlayers();
        }

        [HttpPost]
        public void Post(IEnumerable<Player> players)
        {
            _playerRepository.SavePlayers(players);
        }
    }
}
