using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaultDomain.Commands.CreateSecret;
using VaultDomain.Queries.Secret;
using VaultDomain.ValueObjects;

namespace VaultAppApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SecretsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SecretsController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = this.User.Identity.Name;
            var secrets = await _mediator.Send(new UserSecretsQuery(user));
            return Ok(secrets);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSecretCommand command)
        {
            var user = this.User.Identity.Name;            
            var secrets = await _mediator.Send(new CreateSecretCommand(user, command.Name, command.Value));
            return Ok(secrets);
        }
    }
}