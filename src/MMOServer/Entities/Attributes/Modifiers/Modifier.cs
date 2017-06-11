namespace JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers {

    using JYW.ThesisMMO.Common.Codes;

    internal abstract class Modifier {
        protected internal AttributeCode m_Attribute;

        internal abstract void ApplyOnEntity(Entity entity);
    }
}
