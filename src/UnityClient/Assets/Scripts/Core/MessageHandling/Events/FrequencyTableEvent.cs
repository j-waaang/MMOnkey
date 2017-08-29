using System;
using ExitGames.Client.Photon;
using System.Collections.Generic;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using JYW.ThesisMMO.Common.Codes;

    public static partial class EventOperations {

        public static Action<IEnumerable<FrequencyEntry>> FrequencyTableEvent;

        private static void OnFrequencyTableEvent(EventData eventData) {

            var mins = (float[])eventData.Parameters[(byte)ParameterCode.Mins];
            var maxs = (float[])eventData.Parameters[(byte)ParameterCode.Maxs];
            var freqs = (int[])eventData.Parameters[(byte)ParameterCode.Frequencies];
            var teams = (SkillTarget[])eventData.Parameters[(byte)ParameterCode.Team];

            var freqEntries = new List<FrequencyEntry>();
            for (int i = 0; i < mins.Length; i++) {
                freqEntries.Add(new FrequencyEntry(mins[i], maxs[i], freqs[i], teams[i]));
            }

            Action action = () => FrequencyTableEvent(freqEntries);
            if (NewPlayerEvent == null) {
                EventQueue.Enqueue(action);
            }
            else {
                action();
            }
        }
    }
}