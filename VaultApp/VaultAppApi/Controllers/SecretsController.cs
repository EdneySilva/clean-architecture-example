using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VaultDomain.Commands.CreateSecret;
using VaultDomain.Commands.DeleteSecret;
using VaultDomain.Commands.UpdateSecret;
using VaultDomain.Queries.Secret;

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
        public async Task<IActionResult> GetAsync()
        {
            var user = this.User.Identity!.Name;
            var secrets = await _mediator.Send(new UserSecretsQuery(user));
            if (!secrets.Success)
                return BadRequest(secrets);
            return Ok(secrets);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreateSecretCommand command)
        {
            var user = this.User.Identity!.Name;            
            var secrets = await _mediator.Send(new CreateSecretCommand(user, command.Name, command.Value));
            if (secrets.Metadata.Errors.Any(a => a.ToLower().Contains("secret with the name")))
                return Conflict(secrets);
            if (!secrets.Success) 
                return BadRequest(secrets);
            return Ok(secrets);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateSecretCommand command)
        {
            var user = this.User.Identity!.Name;
            var secrets = await _mediator.Send(new UpdateSecretCommand(user, command.Name, command.Value));
            if (secrets.Metadata.Errors.Any(a => a.ToLower().Contains("not found")))
                return NotFound(secrets);
            if (!secrets.Success)
                return BadRequest(secrets);
            return Ok(secrets);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteSecretCommand command)
        {
            var user = this.User.Identity!.Name;
            var secrets = await _mediator.Send(new DeleteSecretCommand(user, command.Name));
            if (secrets.Metadata.Errors.Any(a => a.ToLower().Contains("not found")))
                return NotFound(secrets);
            if (!secrets.Success)
                return BadRequest(secrets);
            return Ok(secrets);
        }
    }
}