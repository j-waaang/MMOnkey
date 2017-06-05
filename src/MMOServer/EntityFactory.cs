namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Requests;
    using JYW.ThesisMMO.MMOServer.Peers;

    internal static class EntityFactory {

        internal static Entity CreatePeerControlledEntity(MMOPeer peer, EnterWorldRequest operation) {
            var position = GetRandomWorldPosition();
            var maxHealth = GetMaxHealth((AutoAttackCodes) operation.Weapon);
            var entity = new Entity(peer, operation.Name, position, maxHealth);
            return entity;
        }

        private static int GetMaxHealth(AutoAttackCodes weapon) {
            switch (weapon) {
                case AutoAttackCodes.Meele:
                    return 100;
                case AutoAttackCodes.Ranged:
                    return 70;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        private static Vector GetRandomWorldPosition() {
            // TODO: Actually return a random position.
            return Vector.Zero;
        }
    }
}
