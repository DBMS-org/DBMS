namespace Domain.Entities.ExplosiveInventory.Enums
{
    /// <summary>
    /// Emulsion sensitization methods
    /// </summary>
    public enum SensitizationType
    {
        /// <summary>
        /// Chemical sensitization using sodium nitrite solutions
        /// </summary>
        Chemical = 1,

        /// <summary>
        /// Physical sensitization using glass microspheres or perlite
        /// </summary>
        Physical = 2,

        /// <summary>
        /// Combination of chemical and physical sensitization
        /// </summary>
        Hybrid = 3
    }
}
