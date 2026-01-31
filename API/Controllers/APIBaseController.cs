using Application.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIBaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        protected readonly IDispatcher _dispatcher;

        public APIBaseController(ILogger<T> logger,
                                 IDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }
    }
}
