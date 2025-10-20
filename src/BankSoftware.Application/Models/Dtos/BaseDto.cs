namespace BankSoftware.Application.Models.Dtos
{
    /// <summary>
    /// Base dto.
    /// </summary>
    public abstract class BaseDto
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
        public DateTimeOffset? ModifieddAt { get; set; }
    }
}
