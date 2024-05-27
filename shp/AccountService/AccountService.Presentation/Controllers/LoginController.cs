using AccountService.Application.AccountUseCases.Commands;
using AccountService.Application.AccountUseCases.Queries;
using AccountService.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> LogInAsync(LogInRequest request)
        {
            var account = await _mediator.Send(new LogInQuery(
                request.Email, request.Password));

            if (account is null)
            {
                return Unauthorized();
            }

            var tokens = await _mediator.Send(new GetTokensCommand(account));
            return Ok(tokens);
        }
    }
}
