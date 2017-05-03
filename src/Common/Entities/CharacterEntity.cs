namespace JYW.ThesisMMO.Common.Entities {
    using Types;
    public class PlayerCharacterEntity : EntityBase {
        public string Name { get; private set; }
        public Vector Position { get; private set; }
        public PlayerCharacterEntity(string name, Vector position) {
            Identifier = name;
            Name = name;
            Position = position;
        }
        public void Move(Vector newPosition) {
            Position = newPosition;
        }
    }
}
