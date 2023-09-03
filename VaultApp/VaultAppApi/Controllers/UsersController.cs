using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VaultAppApi.Extensions.Security;
using VaultApplication.Users.Commands;
using VaultDomain.Commands.AuthenticateUser;
using VaultDomain.Commands.RegisterUser;
using VaultDomain.ValueObjects;

namespace VaultAppApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, IConfiguration configuration, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserCommand registerUser)
        {
            var commandResult = await _mediator.Send(registerUser);
            return Ok(commandResult);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateUserCommand command)
        {
            var commandResult = await _mediator.Send(command);
            if (commandResult.Success)
                return Ok(new Result(new
                {
                    Token = ((VaultDomain.Entities.User)commandResult.Payload).ExtractJwt(
                        _configuration.GetSection("Token").Value
                    )
                }));
            return BadRequest(commandResult);
        }
    }
}