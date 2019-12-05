using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTGinator.Repositories;

namespace MTGinator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(ILogger<WeatherForecastController> logger, IPlayerRepository playerRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var players = _playerRepository.GetPlayers();
            return Ok(players);
        }

        [HttpPost]
        public ActionResult Post(IEnumerable<Player> players)
        {
            _playerRepository.SavePlayers(players);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            _playerRepository.DeletePlayer(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult Put(Player player)
        {
            _playerRepository.EditPlayer(player);
            return Ok(player);
        }
    }
}
