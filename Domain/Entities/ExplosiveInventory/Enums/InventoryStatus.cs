namespace Domain.Entities.ExplosiveInventory.Enums
{
    /// <summary>
    /// Status of inventory batches in the central warehouse
    /// </summary>
    public enum InventoryStatus
    {
        /// <summary>
        /// Available for allocation and transfer
        /// </summary>
        Available = 1,

        /// <summary>
        /// Partially or fully allocated to transfer requests
        /// </summary>
        Allocated = 2,

        /// <summary>
        /// Expired - cannot be used
        /// </summary>
        Expired = 3,

        /// <summary>
        /// Under quality control investigation
        /// </summary>
        Quarantined = 4,

        /// <summary>
        /// Stock depleted (zero quantity)
        /// </summary>
        Depleted = 5
    }
}
