using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MTGinator.Controllers
{
    [ApiController]
    [Route("api/db")]
    public class DatabaseController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DatabaseController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public FileResult DownloadDb()
        {
            var fileBytes = System.IO.File.ReadAllBytes(_config["DatabasePath"]);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "mtg.db");
        }
    }
}
