namespace JYW.ThesisMMO.Common.Entities {
    public class CharacterEntity : EntityBase {

        public string Name { get; private set; }
        public Vector Position { get; private set; }

        public CharacterEntity(string name, Vector position) {
            Identifier = name;
            Name = name;
            Position = position;
        }
        public void Move(Vector newPosition) {

        }
    }
}
