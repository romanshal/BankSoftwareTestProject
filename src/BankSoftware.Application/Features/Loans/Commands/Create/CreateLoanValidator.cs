using BankSoftware.Application.Features.Loans.Queries.GetAll;
using FluentValidation;

namespace BankSoftware.Application.Features.Loans.Commands.Create
{
    internal sealed class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
    {
        public CreateLoanValidator()
        {
            RuleFor(x => x.Amount).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(x => x.TermValue).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(x => x.InterestValue).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
