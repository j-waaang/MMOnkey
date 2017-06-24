using System;
using JYW.ThesisMMO.Common.ContinueObjects;
using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;
using JYW.ThesisMMO.MMOServer.Targets;

namespace JYW.ThesisMMO.MMOServer.AI {

    internal class BowAutoAttackAI : AIEntity {

        private IntModifier m_DamageMod = new IntModifier(ModifyMode.Addition, Common.Codes.AttributeCode.Health, -17);
        private Entity m_TargetEntity;

        public BowAutoAttackAI(Entity sourceEntity, Entity targetEntity) : base(sourceEntity) {
            m_TargetEntity = targetEntity;
        }

        public override void Update(TimeSpan deltaTime) {
        }

        private void DoDamage() {
            World.Instance.ApplyModifier(m_TargetEntity.Name, m_DamageMod);
            Entity.Die();
        }


    }
}
