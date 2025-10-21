using BankSoftware.Application.Models.CQRS;

namespace BankSoftware.Application.Features.Loans.Commands.Create
{
    public sealed record CreateLoanCommand : ICommand
    {
        public required string Number { get; set; }
        public required decimal Amount { get; set; }
        public required int TermValue { get; set; }
        public required decimal InterestValue { get; set; }
    }
}
