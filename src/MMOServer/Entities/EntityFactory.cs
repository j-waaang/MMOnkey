namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Requests;
    using JYW.ThesisMMO.MMOServer.Peers;

    internal static class EntityFactory {

        internal static Entity CreatePeerControlledEntity(MMOPeer peer, EnterWorldRequest operation) {
            var position = GetRandomWorldPosition();
            var maxHealth = GetMaxHealth((WeaponCode) operation.Weapon);
            var entity = new Entity(peer, operation.Name, position, maxHealth);
            return entity;
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
