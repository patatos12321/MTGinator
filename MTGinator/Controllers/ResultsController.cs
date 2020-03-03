using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MTGinator.Repositories;
using MTGinator.Models;

namespace MTGinator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IResultRepository _resultRepository;

        public ResultsController(ILogger<EventsController> logger, IResultRepository resultRepository)
        {
            _logger = logger;
            _resultRepository = resultRepository;
        }

        [HttpGet]
        public IEnumerable<Result> Get()
        {
            var results = _resultRepository.GetAll();
            return results;
        }

        [HttpGet]
        [Route("{id:int}")]
        public Result Get(int id)
        {
            var result = _resultRepository.GetById(id);
            return result;
        }

        [HttpPost]
        public Result Post(Result result)
        {
            _resultRepository.Save(result);
            return result;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult Delete(int id)
        {
            _resultRepository.Delete(id);
            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}")]
        public Result Put(Result result)
        {
            _resultRepository.Save(result);
            return result;
        }
    }
}
