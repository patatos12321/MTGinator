using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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

        public PlayersController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Player> Get()
        {
            List<Player> players = new List<Player>();
            players.Add(new Player { Name = "Nicolas L.", Score = 0 });
            players.Add(new Player { Name = "Ayrton W.", Score = 0 });
            players.Add(new Player { Name = "Jo R. C.", Score = 0 });

            return players;
        }

        [HttpPost]
        public void Post(IEnumerable<Player> players)
        {
            return;
        }
    }
}
