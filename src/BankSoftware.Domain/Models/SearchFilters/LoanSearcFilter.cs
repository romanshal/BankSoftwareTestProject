using BankSoftware.Domain.Constants.Enums;

namespace BankSoftware.Domain.Models.SearchFilters
{
    public sealed record LoanSearcFilter(
        LoanStatus? Status,
        decimal? MinAmount,
        decimal? MaxAmount,
        int? MinTerm,
        int? MaxTerm);
}
