namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Requests;
    using JYW.ThesisMMO.MMOServer.Peers;
    using Entities.Attributes;

    internal static class EntityFactory {

        internal static Entity CreatePeerControlledEntity(MMOPeer peer, EnterWorldRequest operation) {
            var position = GetRandomWorldPosition();
            var maxHealth = GetMaxHealth((WeaponCode)operation.Weapon);
            var attributes = new Attribute[4];
            attributes[0] = new IntAttribute(maxHealth, AttributeCode.MaxHealth);
            attributes[1] = new HealthAttribute(maxHealth);
            attributes[2] = new ActionStateAttribute();
            attributes[3] = new FloatAttribute(0.2f, AttributeCode.Speed);

            return new Entity(operation.Name, position, attributes, peer);
        }

        internal static Entity CreateAIBot(string name, Vector startPosition) {
            var position = startPosition;
            var maxHealth = GetMaxHealth(WeaponCode.Axe);
            var attributes = new Attribute[4];
            attributes[0] = new IntAttribute(maxHealth, AttributeCode.MaxHealth);
            attributes[1] = new HealthAttribute(maxHealth);
            attributes[2] = new ActionStateAttribute();
            attributes[3] = new FloatAttribute(0.2f, AttributeCode.Speed);

            return new Entity(name, position, attributes, null);
        }

        // TODO: Change design so health does not depend on weapon.
        private static int GetMaxHealth(WeaponCode weapon) {
            switch (weapon) {
                case WeaponCode.Axe:
                    return 100;
                case WeaponCode.Bow:
                    return 70;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        // TODO: Actually return a random position.
        private static Vector GetRandomWorldPosition() {
            return Vector.Zero;
        }
    }
}
