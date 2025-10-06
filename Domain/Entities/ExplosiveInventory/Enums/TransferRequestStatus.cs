namespace Domain.Entities.ExplosiveInventory.Enums
{
    /// <summary>
    /// Transfer request workflow status
    /// </summary>
    public enum TransferRequestStatus
    {
        /// <summary>
        /// Request submitted, awaiting approval
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Approved by Explosive Manager
        /// </summary>
        Approved = 2,

        /// <summary>
        /// Rejected by Explosive Manager
        /// </summary>
        Rejected = 3,

        /// <summary>
        /// In progress - being fulfilled
        /// </summary>
        InProgress = 4,

        /// <summary>
        /// Transfer completed successfully
        /// </summary>
        Completed = 5,

        /// <summary>
        /// Request cancelled
        /// </summary>
        Cancelled = 6
    }
}
