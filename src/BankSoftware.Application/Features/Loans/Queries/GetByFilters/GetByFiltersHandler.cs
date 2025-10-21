using AutoMapper;
using BankSoftware.Application.Interfaces.Repositories;
using BankSoftware.Application.Models;
using BankSoftware.Application.Models.Dtos;
using BankSoftware.Domain.Constants.Enums;
using BankSoftware.Domain.Models;
using BankSoftware.Domain.Models.SearchFilters;
using MediatR;

namespace BankSoftware.Application.Features.Loans.Queries.GetByFilters
{
    internal sealed class GetByFiltersHandler(
        ILoanRepository loanRepository,
        IMapper mapper) : IRequestHandler<GetByFiltersQuery, Result<PaginatedList<LoanDto>>>
    {
        private readonly ILoanRepository _loanRepository = loanRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PaginatedList<LoanDto>>> Handle(GetByFiltersQuery request, CancellationToken cancellationToken)
        {
            var paging = new Paging(request.PageIndex, request.PageSize);
            var filter = new LoanSearcFilter(
                request.IsPublished.HasValue ? request.IsPublished.Value ? LoanStatus.Published : LoanStatus.Unpublished : null,
                request.MinAmount,
                request.MaxAmount,
                request.MinTerm,
                request.MaxTerm);

            var (loans, total) = await _loanRepository.GetByFiltersAsync(
                filter: filter,
                paging: paging,
                tracking: false,
                cancellationToken);

            if (!loans.Any())
            {
                return PaginatedList<LoanDto>.Empty(request.PageIndex, request.PageSize);
            }

            var loanDtos = _mapper.Map<IEnumerable<LoanDto>>(loans);

            return PaginatedList<LoanDto>.Create(loanDtos, request.PageIndex, request.PageSize, total);
        }
    }
}
