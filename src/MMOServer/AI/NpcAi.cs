namespace JYW.ThesisMMO.MMOServer.AI {

    class NpcAi : AIEntity {

        private PlayerEntity m_Entity;

        internal NpcAi(PlayerEntity npc) {
            m_Entity = npc;
        }

        internal override void Tick() {
        }
    }
}
