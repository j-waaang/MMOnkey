using System;
using JYW.ThesisMMO.Common.ContinueObjects;
using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;
using JYW.ThesisMMO.MMOServer.Targets;

namespace JYW.ThesisMMO.MMOServer.AI {

    internal class FireStormAI : AIEntity {

        private ActionContinueCondition m_DamageTicker = new ContinueTick(new System.TimeSpan(0, 0, 0, 0, 500), 10);
        private IntModifier m_DamageMod = new IntModifier(ModifyMode.Addition, Common.Codes.AttributeCode.Health, -10);
        private Target m_AoeTarget;

        public FireStormAI(Entity entity) : base(entity) {
            m_AoeTarget = new CircleAreaTarget() {
                AreaTargetOption = AreaTargetOption.IgnoreSource,
                Center = Entity.Position,
                Radius = 3.5f,
                SourceName = Entity.Name,
            };

            m_DamageTicker.ContinueEvent += Tick;
            m_DamageTicker.Start();
        }
        

        private void Tick(CallReason callReason) {
            DoDamage();

            if (callReason == CallReason.LastTick) {
                m_DamageTicker.ContinueEvent -= Tick;
                Entity.Die();
            }
        }

        private void DoDamage() {
            World.Instance.ApplyModifier(m_AoeTarget, m_DamageMod);
        }

        public override void Update(TimeSpan deltaTime) {
        }
    }
}
