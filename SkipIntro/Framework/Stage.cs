namespace Pathoschild.Stardew.SkipIntro.Framework
{
    /// <summary>A step in the mod logic.</summary>
    internal enum Stage
    {
        /// <summary>No action needed.</summary>
        None,

        /// <summary>Skip the initial intro.</summary>
        SkipIntro,

        /// <summary>The co-op menu is waiting for a connection.</summary>
        WaitingForConnection
    }
}
