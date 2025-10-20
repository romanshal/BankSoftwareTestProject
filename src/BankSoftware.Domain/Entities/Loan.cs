using BankSoftware.Domain.Constants.Enums;

namespace BankSoftware.Domain.Entities
{
    /// <summary>
    /// Main entity of bank software.
    /// </summary>
    public class Loan : BaseEntity
    {
        /// <summary>
        /// Status of loan: Published/Unpublished.
        /// </summary>
        public LoanStatus Status { get; set; }

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
