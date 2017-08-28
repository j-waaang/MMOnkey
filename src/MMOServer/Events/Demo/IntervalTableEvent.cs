using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using System.Collections.Generic;

namespace JYW.ThesisMMO.MMOServer.Events.Demo {

    using CSAIM;
    using JYW.ThesisMMO.Common.Codes;

    internal class IntervalTableEvent {

        public IntervalTableEvent(List<MsInInterval> intervals) {
            var x = intervals.Count;
            Mins = new float[x];
            Maxs = new float[x];
            Frequencies = new int[x];
            Teams = new int[x];

            var i = 0;
            foreach (var element in intervals) {
                Mins[i] = element.MinDistance;
                Maxs[i] = element.MaxDistance;
                Frequencies[i] = element.MilliSeconds;
                Teams[i] = (int)element.Target;
            }
        }

        [DataMember(Code = (byte)ParameterCode.Mins)]
        public readonly float[] Mins;

        [DataMember(Code = (byte)ParameterCode.Maxs)]
        public readonly float[] Maxs;

        [DataMember(Code = (byte)ParameterCode.CurrentHealth)]
        public readonly int[] Frequencies;

        [DataMember(Code = (byte)ParameterCode.Team)]
        public readonly int[] Teams;

        public IEventData ToEventData() {
            return new EventData((byte)EventCode.FrequencyTable, this);
        }
    }
}