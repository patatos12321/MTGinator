﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTGinator.Repositories;
using MTGinator.Models;
using MTGinator.Commands;
using MediatR;

namespace MTGinator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IRepository<Event> _eventRepository;
        private readonly IMediator _mediator;

        public EventsController(ILogger<EventsController> logger, IRepository<Event> eventRepository, IMediator mediator)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var events = _eventRepository.GetAll();
            return Ok(events);
        }

        [HttpGet]
        [Route("{id:int}")]
        public ActionResult Get(int id)
        {
            var events = _eventRepository.GetById(id);
            return Ok(events);
        }

        [HttpGet]
        [Route("{id:int}/next-pairings")]
        public ActionResult Pairings(int id)
        {
            var command = new GetNextRound(id);
            return Ok(_mediator.Send(command));
        }

        [HttpPost]
        public ActionResult Post(Event @event)
        {
            _eventRepository.Save(@event);
            return Ok(@event);
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
        public ActionResult Put(Event @event)
        {
            _eventRepository.Save(@event);
            return Ok(@event);
        }
    }
}
