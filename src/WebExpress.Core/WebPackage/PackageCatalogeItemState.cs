namespace WebExpress.Core.WebPackage
{
    public enum PackageCatalogeItemState
    {
        /// <summary>
        /// Das Paket ist verfügbar, jedoch noch nicht vom WebExpress geladen.
        /// </summary>
        Available,

        /// <summary>
        /// Das Paket wurde geladen und steht zur Nutzung bereit.
        /// </summary>
        Active,

        /// <summary>
        /// Das Paket wurde deaktiviert. Die Nutzung des Paketes ist nicht möglich.
        /// </summary>
        Disable
    }
}
