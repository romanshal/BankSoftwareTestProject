using BankSoftware.Application.Models.CQRS;

namespace BankSoftware.Application.Features.Loans.Commands.ChangeStatus
{
    public sealed record ChangeStatusCommand : ICommand
    {
        public required Guid Id { get; set; }
        public bool IsPublished { get; set; }
    }
}
