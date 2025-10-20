using BankSoftware.Application.Models;
using BankSoftware.Application.Models.CQRS;
using BankSoftware.Application.Models.Dtos;

namespace BankSoftware.Application.Features.Loans.Queries.GetByFilters
{
    public record GetByFiltersQuery : IQuery<PaginatedList<LoanDto>>
    {
        public bool? IsPublished { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; } 
        public int? MinTerm { get; set; }
        public int? MaxTerm { get; set; } 
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
