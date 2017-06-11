namespace JYW.ThesisMMO.MMOServer.Entities {

    using JYW.ThesisMMO.Common.Types;

    /// <summary> 
    /// Target is either defined by a vector or a entity name.
    /// </summary>
    internal struct Target {

        public TargetType TargetType { get; set; }
        public string TargetName { get; set; }
        public Vector TargetPosition { get; set; }
    }
}
