using AutoMapper;
using BankSoftware.Application.Interfaces.Repositories;
using BankSoftware.Application.Models;
using BankSoftware.Application.Models.Dtos;
using BankSoftware.Domain.Models;
using MediatR;

namespace BankSoftware.Application.Features.Loans.Queries.GetAll
{
    internal class GetAllLoansHandler(
        ILoanRepository loanRepository,
        IMapper mapper) : IRequestHandler<GetAllLoansQuery, Result<PaginatedList<LoanDto>>>
    {
        private readonly ILoanRepository _loanRepository = loanRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PaginatedList<LoanDto>>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
        {
            var paging = new Paging(request.PageIndex, request.PageSize);
            var (loans, total) = await _loanRepository.GetAllAsync(paging, cancellationToken);
            if (!loans.Any())
            {
                return PaginatedList<LoanDto>.Empty(request.PageIndex, request.PageSize);
            }

            var loanDtos = _mapper.Map<IEnumerable<LoanDto>>(loans);

            return PaginatedList<LoanDto>.Create(loanDtos, request.PageIndex, request.PageSize, total);
        }
    }
}
