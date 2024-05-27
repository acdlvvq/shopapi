using AccountService.Application.AccountUseCases.Commands;
using AccountService.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RefreshController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var response = await _mediator.Send(
                new RefreshTokenCommand(request.RefreshToken)); 

           return response is null ? Unauthorized() : Ok(response);
        }
    }
}
