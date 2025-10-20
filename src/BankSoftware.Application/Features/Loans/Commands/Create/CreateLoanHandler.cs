using AutoMapper;
using BankSoftware.Application.Interfaces.Repositories;
using BankSoftware.Application.Models;
using BankSoftware.Domain.Constants.Errors;
using BankSoftware.Domain.Constants.Errors.ErrorMessages;
using BankSoftware.Domain.Entities;
using MediatR;

namespace BankSoftware.Application.Features.Loans.Commands.Create
{
    internal sealed class CreateLoanHandler(
        ILoanRepository loanRepository,
        IMapper mapper) : IRequestHandler<CreateLoanCommand, Result>
    {
        private readonly ILoanRepository _loanRepository = loanRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(CreateLoanCommand request, CancellationToken cancellationToken)
        {
            var loan = _mapper.Map<Loan>(request);

            var affectedRows = await _loanRepository.AddAsync(loan, cancellationToken);

            return affectedRows == 0 ? Result.Failure(Error.CantCreate(LoanErrorMessages.CantCreate)) : Result.Success();
        }
    }
}
