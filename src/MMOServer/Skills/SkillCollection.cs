using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer.Skills {

    using CSAIM;
    using JYW.ThesisMMO.Common.Codes;

    internal class SkillCollection {

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public ActionCode[] Skills {
            get { return m_SkillStates.Keys.ToArray(); }
        }

        private const string SkillDataNamespace = "JYW.ThesisMMO.MMOServer.Skills.";
        private const int MaxSkills = 3;
        private readonly Dictionary<ActionCode, SkillData> m_SkillStates = new Dictionary<ActionCode, SkillData>();

        public SkillCollection(int[] skills) {
            skills = skills.Distinct().ToArray();
            Debug.Assert(skills.Count() <= MaxSkills, "Tried to initialize skill collection with more skills than allowed.");
            
            foreach(var skill in skills) {
                var code = (ActionCode)skill;
                var type = SkillDataNamespace + code + "Data";
                var typed = Type.GetType(type);
                Debug.Assert(typed != null, string.Format("{0} skill doesn't exist.", type));

                var instance = Activator.CreateInstance(typed) as SkillData;
                m_SkillStates.Add(code, instance);
            }
        }

        public IEnumerable<MsInInterval> GetSkillConsistencyRequirements() {
            foreach(var data in m_SkillStates.Values) {
                var req = data.GetConsistencyRequirement();
                if (req.IsZero) { continue; }
                yield return data.GetConsistencyRequirement();
            }
            yield break;
        }

        public bool CanActivateSkill(ActionCode skill) {
            if (!m_SkillStates.ContainsKey(skill)) { log.InfoFormat("skill coll doesn't contain skill {0}", skill); }

            if (!m_SkillStates.ContainsKey(skill)) { return false; }
            return m_SkillStates[skill].CanCast;
        }

        public void SetSkillOnCooldown(ActionCode skill) {
            Debug.Assert(m_SkillStates.ContainsKey(skill));
            m_SkillStates[skill].SetOnCooldown();
        }
    }
}
