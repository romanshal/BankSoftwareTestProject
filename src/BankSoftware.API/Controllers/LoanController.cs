using BankSoftware.API.Extensions;
using BankSoftware.Application.Features.Loans.Commands.ChangeStatus;
using BankSoftware.Application.Features.Loans.Commands.Create;
using BankSoftware.Application.Features.Loans.Queries.GetAll;
using BankSoftware.Application.Features.Loans.Queries.GetByFilters;
using BankSoftware.Application.Models;
using BankSoftware.Application.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankSoftware.API.Controllers
{
    [ApiController]
    [Route("api/v1/loans")]
    public class LoanController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<LoanDto>>> GetAllAsync(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllLoansQuery 
            { 
                PageIndex = pageIndex, 
                PageSize = pageSize 
            }, cancellationToken);

            return this.MatchResponse(result);
        }

        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<LoanDto>>> GetByFiltersAsync(
            [FromQuery] int pageIndex,
            [FromQuery] int pageSize,
            [FromQuery] bool? isPublished = null,
            [FromQuery] decimal? minAmount = null,
            [FromQuery] decimal? maxAmount = null,
            [FromQuery] int? minTerm = null,
            [FromQuery] int? maxTerm = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetByFiltersQuery
            {
                IsPublished = isPublished,
                MinAmount = minAmount,
                MaxAmount = maxAmount,
                MinTerm = minTerm,
                MaxTerm = maxTerm,
                PageIndex = pageIndex,
                PageSize = pageSize,
            }, cancellationToken);

            return this.MatchResponse(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync(
            [FromBody] CreateLoanCommand command, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return this.MatchResponse(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeStatusAsync(
            [FromBody] ChangeStatusCommand command,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return this.MatchResponse(result);
        }
    }
}
