namespace BankSoftware.Application.Models.Dtos
{
    /// <summary>
    /// Loan dto.
    /// </summary>
    public sealed class LoanDto : BaseDto
    {
        /// <summary>
        /// Status of loan.
        /// </summary>
        public required string Status { get; set; }

        /// <summary>
        /// Uniq number of loan.
        /// </summary>
        public required string Number { get; set; }

        /// <summary>
        /// Sum of loan.
        /// </summary>
        public required decimal Amount { get; set; }

        /// <summary>
        /// Loan term.
        /// </summary>
        public required int TermValue { get; set; }

        /// <summary>
        /// Interest rate.
        /// </summary>
        public required decimal InterestValue { get; set; }
    }
}
