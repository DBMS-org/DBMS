namespace Domain.Entities.ExplosiveInventory.Enums
{
    /// <summary>
    /// Emulsion explosive grades
    /// </summary>
    public enum EmulsionGrade
    {
        /// <summary>
        /// Standard density emulsion (1150-1250 kg/m³)
        /// </summary>
        Standard = 1,

        /// <summary>
        /// High density emulsion (&gt;1250 kg/m³)
        /// </summary>
        HighDensity = 2,

        /// <summary>
        /// Low density emulsion (800-1000 kg/m³)
        /// </summary>
        LowDensity = 3,

        /// <summary>
        /// Water-resistant formulation with blue tint
        /// </summary>
        WaterResistant = 4
    }
}
