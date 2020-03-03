using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTGinator.Repositories;
using MTGinator.Models;
using MTGinator.Commands;
using MediatR;
using MTGinator.Extensions;

namespace MTGinator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IMediator _mediator;

        public EventsController(ILogger<EventsController> logger, IEventRepository eventRepository, IMediator mediator)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public IEnumerable<Event> Get()
        {
            var events = _eventRepository.GetAll().OrderByDescending(e => e.Date);
            return events;
        }

        [HttpGet]
        [Route("{id:int}")]
        public Event Get(int id)
        {
            var @event = _eventRepository.GetById(id);
            return @event;
        }

        [HttpPost]
        public Event Post(Event @event)
        {
            _eventRepository.Save(@event);
            return @event;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            _eventRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}")]
        public Event Put(Event @event)
        {
            _eventRepository.Save(@event);
            return @event;
        }

        [HttpGet]
        [Route("{id:int}/Players")]
        public IEnumerable<Player> PlayersInRandomOrder(int id)
        {
            var @event = _eventRepository.GetById(id);
            @event.ParticipatingPlayers.ShuffleManyTimes(8);
            return @event.ParticipatingPlayers;
        }

        [HttpGet]
        [Route("{id:int}/next-round")]
        public async Task<Round> NextRound(int id)
        {
            var command = new GetNextSwissRound(id);
            var round = await _mediator.Send(command);
            return round;
        }

        [HttpPost]
        [Route("{id:int}/round")]
        public Round SaveRound(int id, Round round)
        {
            var @event = _eventRepository.GetById(id);
            @event.Rounds.Add(round);
            _eventRepository.Save(@event);
            return round;
        }

        [HttpGet]
        [Route("{id:int}/results")]
        public async Task<IEnumerable<Result>> Results(int id)
        {
            var command = new GetEventResults(id);
            var results = await _mediator.Send(command);
            return results;
        }
    }
}
