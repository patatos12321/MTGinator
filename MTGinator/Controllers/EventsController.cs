using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTGinator.Repositories;

namespace MTGinator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IRepository<Event> _eventRepository;

        public EventsController(ILogger<EventsController> logger, IRepository<Event> eventRepository)
        {
            _logger = logger;
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            //var events = _eventRepository.GetAll();
            var events = new List<Event>();
            events.Add(new Event
            {
                Name = "Test",
                Date = DateTime.Now,
                ImagePath = "https://media.magic.wizards.com/xrtIRoU5ub.jpg",
                ParticpatingPlayers = new List<Player>
                {
                    new Player
                    {
                        Name = "nic"
                    }
                },
                Id = 1
            });

            events.Add(new Event
            {
                Name = "Autre test",
                Date = DateTime.Now.AddDays(-12),
                ImagePath = "https://am21.akamaized.net/tms/cnt/uploads/2019/07/Throne-of-Eldrane-information.jpg",
                ParticpatingPlayers = new List<Player>
                {
                    new Player
                    {
                        Name = "nic"
                    },
                    new Player
                    {
                        Name = "jo"
                    }
                },
                Id = 2
            });

            events.Add(new Event
            {
                Name = "Special edition",
                Date = DateTime.Now.AddDays(-12),
                ImagePath = "https://am21.akamaized.net/tms/cnt/uploads/2019/07/Throne-of-Eldrane-information.jpg",
                ParticpatingPlayers = new List<Player>
                {
                    new Player
                    {
                        Name = "nic"
                    },
                    new Player
                    {
                        Name = "jo"
                    }
                },
                Id = 2
            });
            return Ok(events);
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
