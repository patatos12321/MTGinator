using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTGinator.Repositories;
using MTGinator.Models;

namespace MTGinator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IRepository<Player> _playerRepository;

        public PlayersController(ILogger<PlayersController> logger, IRepository<Player> playerRepository)
        {
            _logger = logger;
            _playerRepository = playerRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var players = _playerRepository.GetAll();
            return Ok(players);
        }

        [HttpPost]
        public ActionResult Post(IEnumerable<Player> players)
        {
            _playerRepository.Save(players);
            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            _playerRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult Put(Player player)
        {
            _playerRepository.Save(player);
            return Ok(player);
        }
    }
}
