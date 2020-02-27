using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTGinator.Commands;
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
        private readonly IMediator _mediator;

        public PlayersController(ILogger<PlayersController> logger, IRepository<Player> playerRepository, IMediator mediator)
        {
            _logger = logger;
            _playerRepository = playerRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Player>> Get()
        {
            return await _mediator.Send(new GetPlayers());
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
