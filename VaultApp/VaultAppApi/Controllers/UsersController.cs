using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaultApplication.Users.Commands;
using VaultDomain.Commands.RegisterUser;

namespace VaultAppApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserCommand registerUser)
        {
            var commandResult = await _mediator.Send(registerUser);
            return Ok(commandResult);
        }
    }
}