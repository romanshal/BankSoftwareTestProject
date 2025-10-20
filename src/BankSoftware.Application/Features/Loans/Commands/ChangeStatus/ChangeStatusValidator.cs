using FluentValidation;

namespace BankSoftware.Application.Features.Loans.Commands.ChangeStatus
{
    internal sealed class ChangeStatusValidator : AbstractValidator<ChangeStatusCommand>
    {
        public ChangeStatusValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
