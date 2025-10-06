namespace Domain.Entities.ExplosiveInventory.Enums
{
    /// <summary>
    /// Quality control approval status
    /// </summary>
    public enum QualityStatus
    {
        /// <summary>
        /// Approved for use
        /// </summary>
        Approved = 1,

        /// <summary>
        /// Awaiting quality control review
        /// </summary>
        Pending = 2,

        /// <summary>
        /// Failed quality control - cannot be used
        /// </summary>
        Rejected = 3
    }
}
