namespace JYW.ThesisMMO.MMOServer {

using JYW.ThesisMMO.Common.Types;

    public class EntityPositionMessage
    {
        public EntityPositionMessage(string source, Vector position)
        {
            this.source = source;
            this.position = position;
        }
        public readonly Vector position;
        public readonly string source;
    }
}
