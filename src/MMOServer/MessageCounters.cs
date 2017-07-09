using ExitGames.Diagnostics.Counter;

namespace JYW.ThesisMMO.MMOServer {

    /// <summary>
    /// Counters that keep track of the amount of messages sent and received from item channels.
    /// </summary>
    public static class MessageCounters {
        /// <summary>
        /// Used to count how many messages were received by InterestAreas (and sometimes items).
        /// </summary>
        public static readonly CountsPerSecondCounter CounterReceive = new CountsPerSecondCounter("ItemMessage.Receive");

        /// <summary>
        /// Used to count how many messages were sent by items (and sometimes InterestAreas).
        /// </summary>
        public static readonly CountsPerSecondCounter CounterSend = new CountsPerSecondCounter("ItemMessage.Send");
    }
}