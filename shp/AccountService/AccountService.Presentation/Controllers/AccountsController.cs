using MediatR;
using Microsoft.AspNetCore.Mvc;
using AccountService.Presentation.Contracts;
using AccountService.Application.AccountUseCases.Queries;
using AccountService.Application.AccountUseCases.Commands;
using Microsoft.AspNetCore.Authorization;

namespace AccountService.Presentation.Controllers
{
    [Authorize(Policy = IdentityData.AdminUserPolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var accounts = await _mediator.Send(new GetAllAccountsQuery());

            return Ok(accounts);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddAsync(AddAccountRequest request)
        {
            var response = await _mediator.Send(new AddAccountCommand(
                request.Email, request.Username, request.Password, request.Role));

            return Ok(response);
        }
    }
}
