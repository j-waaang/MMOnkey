using JYW.ThesisMMO.Common.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYW.ThesisMMO.MMOServer.Entities {
    internal class SkillState {

        public ActionCode Skill { get; }

        public SkillState(ActionCode skill) {
            Skill = skill;
        }
    }
}
