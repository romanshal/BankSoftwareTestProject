using BankSoftware.Domain.Entities;
using BankSoftware.Domain.Models;
using BankSoftware.Domain.Models.SearchFilters;

namespace BankSoftware.Application.Interfaces.Repositories
{
    /// <summary>
    /// Loan repository.
    /// </summary>
    public interface ILoanRepository
    {
        /// <summary>
        /// Get all paginated loans in database.
        /// </summary>
        /// <param name="paging">Paging.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paginated list of loans.</returns>
        Task<(IEnumerable<Loan> loans, int total)> GetAllAsync(
            Paging paging,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get loan by id.
        /// </summary>
        /// <param name="id">Loan id.</param>
        /// <param name="tracking">Is tracking entities.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Loan entity or null</returns>
        Task<Loan?> GetAsync(Guid id, bool tracking = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get loans from database from filter.
        /// </summary>
        /// <param name="filter">Filters.</param>
        /// <param name="paging">Paging.</param>
        /// <param name="tracking">Is tracking entities.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paginated list of loans.</returns>
        Task<(IEnumerable<Loan> loans, int total)> GetByFiltersAsync(
            LoanSearcFilter filter,
            Paging paging,
            bool tracking = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new loan entity to database.
        /// </summary>
        /// <param name="loan">Loan entity</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of state entities written to database.</returns>
        Task<int> AddAsync(Loan loan, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update loan entity in database.
        /// </summary>
        /// <param name="loan">Loan entity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of state entities written to database.</returns>
        Task<int> UpdateAsync(Loan loan, CancellationToken cancellationToken = default);
    }
}
