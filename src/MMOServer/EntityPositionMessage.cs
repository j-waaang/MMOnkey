namespace JYW.ThesisMMO.MMOServer {

using JYW.ThesisMMO.Common.Types;

    public class EntityPositionMessage
    {
        public EntityPositionMessage(string source, Vector position)
        {
            this.Source = source;
            this.Position = position;
        }
        public Vector Position { get; private set; }
        public string Source { get; private set; }
    }
}
