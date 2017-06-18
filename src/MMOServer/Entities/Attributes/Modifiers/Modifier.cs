namespace JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers {

    using JYW.ThesisMMO.Common.Codes;
    using Targets;

    internal abstract class Modifier {
        protected internal AttributeCode m_Attribute;

        public Target Target { get; protected set; }

        public abstract void ApplyEffect(Entity entity);
    }
}
