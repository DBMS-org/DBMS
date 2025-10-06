namespace Domain.Entities.ExplosiveInventory.Enums
{
    /// <summary>
    /// Fume classification for explosive materials safety
    /// </summary>
    public enum FumeClass
    {
        /// <summary>
        /// Class 1 - Safe, properly balanced (recommended)
        /// </summary>
        Class1 = 1,

        /// <summary>
        /// Class 2 - Acceptable with caution
        /// </summary>
        Class2 = 2,

        /// <summary>
        /// Class 3 - Hazardous, requires immediate attention
        /// </summary>
        Class3 = 3
    }
}
