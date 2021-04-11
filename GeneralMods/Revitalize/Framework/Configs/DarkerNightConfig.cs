namespace Omegasis.Revitalize.Framework.Configs
{
    /// <summary>
    /// Deals with configurations for darker night.
    /// </summary>
    public class DarkerNightConfig
    {
        /// <summary>
        /// Is darker night enabled?
        /// </summary>
        public bool Enabled;
        /// <summary>
        /// The intensity for how dark it gets at night.
        /// </summary>
        public float DarknessIntensity;
        public DarkerNightConfig()
        {
            this.Enabled = true;
            this.DarknessIntensity = .9f;
        }
    }
}
