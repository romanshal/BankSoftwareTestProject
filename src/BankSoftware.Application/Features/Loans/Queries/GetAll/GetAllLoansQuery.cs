using BankSoftware.Application.Models;
using BankSoftware.Application.Models.CQRS;
using BankSoftware.Application.Models.Dtos;

namespace BankSoftware.Application.Features.Loans.Queries.GetAll
{
    public sealed record GetAllLoansQuery : IQuery<PaginatedList<LoanDto>>
    {
        public required int PageIndex { get; set; }
        public required int PageSize { get; set; }
    }
}
