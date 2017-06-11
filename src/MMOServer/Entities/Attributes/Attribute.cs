namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {

    using ExitGames.Logging;

    using JYW.ThesisMMO.Common.Codes;

    internal abstract class Attribute {

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        protected Entity m_Entity;

        public AttributeCode AttributeCode { get; protected set; }

        internal Attribute(AttributeCode attributeCode) {
            AttributeCode = attributeCode;
        }

        internal void SetEntity(Entity entity) {
            m_Entity = entity;
        }
    }
}
