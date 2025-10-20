namespace BankSoftware.Domain.Entities
{
    /// <summary>
    /// Base entity.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Created at.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Modified at.
        /// </summary>
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
