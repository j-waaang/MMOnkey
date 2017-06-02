namespace JYW.ThesisMMO.MMOServer.AI {

    /// <summary> 
    /// Base class for AIs to deerive from.
    /// </summary>
    internal abstract class AIEntity {

        /// <summary> 
        /// Called by AIModule's loop.
        /// </summary>
        internal abstract void Update();
    }
}
