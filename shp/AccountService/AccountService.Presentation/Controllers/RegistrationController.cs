using AccountService.Application.AccountUseCases.Commands;
using AccountService.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterAsync(RegistrationRequest request)
        {
            var added = await _mediator.Send(new RegisterAccountCommand(
                request.Email, request.Username, request.Password));

            return added == 0 ? BadRequest() : Ok();
        }
    }
}
