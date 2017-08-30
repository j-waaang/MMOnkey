using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using System.Collections.Generic;

namespace JYW.ThesisMMO.MMOServer.Events.Demo {

    using CSAIM;
    using ExitGames.Logging;
    using JYW.ThesisMMO.Common.Codes;

    internal class IntervalTableEvent {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public IntervalTableEvent(List<MsInInterval> intervals) {

            var x = intervals.Count;
            Mins = new float[x];
            Maxs = new float[x];
            Frequencies = new int[x];
            Teams = new int[x];

            var i = 0;
            foreach (var element in intervals) {
                //log.InfoFormat("Starting to conver interval {0}", element.ToString());

                Mins[i] = element.MinDistance;
                Maxs[i] = element.MaxDistance;
                Frequencies[i] = element.MilliSeconds;
                Teams[i] = (int)element.Target;
                i++;
            }
        }

        [DataMember(Code = (byte)ParameterCode.Mins)]
        public readonly float[] Mins;

        [DataMember(Code = (byte)ParameterCode.Maxs)]
        public readonly float[] Maxs;

        [DataMember(Code = (byte)ParameterCode.Frequencies)]
        public readonly int[] Frequencies;

        [DataMember(Code = (byte)ParameterCode.Team)]
        public readonly int[] Teams;

        public IEventData ToEventData() {
            //log.InfoFormat("Sending freqtable event with {0} entries", Mins.Length);
            //log.InfoFormat("Mins {0}, Maxs {1}, Freqs {2}, Teams {3}", string.Join(";", Mins), string.Join(";", Maxs), string.Join(";", Frequencies), string.Join(";", Teams));

            return new EventData((byte)EventCode.FrequencyTable, this);
        }
    }
}