using AutoMapper;
using BankSoftware.Application.Interfaces.Cache;
using BankSoftware.Application.Interfaces.Repositories;
using BankSoftware.Application.Models;
using BankSoftware.Application.Models.Dtos;
using BankSoftware.Domain.Entities;
using BankSoftware.Domain.Models;
using MediatR;

namespace BankSoftware.Application.Features.Loans.Queries.GetAll
{
    internal class GetAllLoansHandler(
        ILoanRepository loanRepository,
        IKeyFactory keyFactory,
        ICacheService cache,
        IMapper mapper) : IRequestHandler<GetAllLoansQuery, Result<PaginatedList<LoanDto>>>
    {
        private readonly ILoanRepository _loanRepository = loanRepository;
        private readonly IKeyFactory _keyFactory = keyFactory;
        private readonly ICacheService _cache = cache;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<PaginatedList<LoanDto>>> Handle(GetAllLoansQuery request, CancellationToken cancellationToken)
        {
            var key = _keyFactory.Key(request);

            var cached = await _cache.GetAsync<IEnumerable<LoanDto>>(key);
            if (cached is not null)
            {
                return PaginatedList<LoanDto>.Create(cached, request.PageIndex, request.PageSize);
            }

            var paging = new Paging(request.PageIndex, request.PageSize);
            var loans = await _loanRepository.GetAllAsync(paging, cancellationToken);
            if (!loans.Any())
            {
                return PaginatedList<LoanDto>.Empty(request.PageIndex, request.PageSize);
            }

            var loanDtos = _mapper.Map<IEnumerable<LoanDto>>(loans);

            await _cache.SetAsync(key, loanDtos);

            return PaginatedList<LoanDto>.Create(loanDtos, request.PageIndex, request.PageSize);
        }
    }
}
