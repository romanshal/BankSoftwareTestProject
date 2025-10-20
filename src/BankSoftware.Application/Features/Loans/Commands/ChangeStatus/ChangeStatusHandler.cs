using BankSoftware.Application.Interfaces.Repositories;
using BankSoftware.Application.Models;
using BankSoftware.Domain.Constants.Enums;
using BankSoftware.Domain.Constants.Errors;
using BankSoftware.Domain.Constants.Errors.ErrorMessages;
using MediatR;

namespace BankSoftware.Application.Features.Loans.Commands.ChangeStatus
{
    internal sealed class ChangeStatusHandler(
        ILoanRepository loanRepository) : IRequestHandler<ChangeStatusCommand, Result>
    {
        private readonly ILoanRepository _loanRepository = loanRepository;

        public async Task<Result> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var loan = await _loanRepository.GetAsync(request.Id, cancellationToken: cancellationToken);
            if(loan is null)
            {
                return Result.Failure(Error.NotFound(LoanErrorMessages.NotFound));
            }

            loan.Status = request.IsPublished ? LoanStatus.Published : LoanStatus.Unpublished;

            var affectedRows = await _loanRepository.UpdateAsync(loan, cancellationToken);
            if(affectedRows == 0)
            {
                return Result.Failure(Error.CantUpdate(LoanErrorMessages.CantUpdate));
            }

            return Result.Success();
        }
    }
}
