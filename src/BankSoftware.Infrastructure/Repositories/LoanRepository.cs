using BankSoftware.Application.Interfaces.Repositories;
using BankSoftware.Domain.Entities;
using BankSoftware.Domain.Models;
using BankSoftware.Domain.Models.SearchFilters;
using BankSoftware.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BankSoftware.Infrastructure.Repositories
{
    /// <summary>
    /// Loan repository.
    /// </summary>
    /// <param name="context">Database context</param>
    internal class LoanRepository(BankDbContext context) : ILoanRepository
    {
        private readonly BankDbContext _context = context;

        /// <summary>
        /// Get all paginated loans in database.
        /// </summary>
        /// <param name="paging">Paging.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paginated list of loans.</returns>
        public async Task<(IEnumerable<Loan> loans, int total)> GetAllAsync(
            Paging paging,
            CancellationToken cancellationToken = default)
        {
            var loans = await _context.Loans
                .AsNoTracking()
                .OrderByDescending(o => o.CreatedAt)
                .Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync(cancellationToken);

            var total = await _context.Loans
                .AsNoTracking()
                .CountAsync(cancellationToken);

            return (loans, total);
        }


        /// <summary>
        /// Get loan by id.
        /// </summary>
        /// <param name="id">Loan id.</param>
        /// <param name="tracking">Is tracking entities.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Loan entity or null</returns>
        public async Task<Loan?> GetAsync(
            Guid id,
            bool tracking = true,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Loans.AsQueryable();

            if (!tracking) query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync(loan => loan.Id == id, cancellationToken);
        }

        /// <summary>
        /// Get loans from database from filter.
        /// </summary>
        /// <param name="filter">Filters.</param>
        /// <param name="paging">Paging.</param>
        /// <param name="tracking">Is tracking entities.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paginated list of loans.</returns>
        public async Task<(IEnumerable<Loan> loans, int total)> GetByFiltersAsync(
            LoanSearcFilter filter,
            Paging paging,
            bool tracking = true,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Loans.AsQueryable();

            if (!tracking) query = query.AsNoTracking();

            if (filter.Status.HasValue) query = query.Where(loan => loan.Status == filter.Status.Value);

            if (filter.MinAmount.HasValue) query = query.Where(loan => loan.Amount >= filter.MinAmount.Value);
            if (filter.MaxAmount.HasValue) query = query.Where(loan => loan.Amount <= filter.MaxAmount.Value);

            if (filter.MinTerm.HasValue) query = query.Where(loan => loan.TermValue >= filter.MinTerm.Value);
            if (filter.MaxTerm.HasValue) query = query.Where(loan => loan.TermValue >= filter.MaxTerm.Value);

            var loans = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((paging.PageIndex - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync(cancellationToken);

            var total = await query.CountAsync(cancellationToken);

            return (loans, total);
        }

        /// <summary>
        /// Add new loan entity to database.
        /// </summary>
        /// <param name="loan">Loan entity</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of state entities written to database.</returns>
        public async Task<int> AddAsync(Loan loan, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(loan, nameof(loan));

            await _context.Loans.AddAsync(loan, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Update loan entity in database.
        /// </summary>
        /// <param name="loan">Loan entity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Number of state entities written to database.</returns>
        public async Task<int> UpdateAsync(Loan loan, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(loan, nameof(loan));

            _context.Update(loan);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
