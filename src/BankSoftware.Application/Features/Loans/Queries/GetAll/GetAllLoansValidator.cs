using FluentValidation;

namespace BankSoftware.Application.Features.Loans.Queries.GetAll
{
    internal sealed class GetAllLoansValidator : AbstractValidator<GetAllLoansQuery>
    {
        public GetAllLoansValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
