﻿using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.CSAIM {

    using ActionObjects.SkillRequests;
    using Common.Codes;
    using Common.Types;
    using Entities.Attributes;
    using Events;
    using ExitGames.Logging;

    internal class PositionFilter {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private static readonly Stopwatch Time = new Stopwatch();
        private static readonly SendParameters PositionSendParameters = new SendParameters() { ChannelId = (byte)ChannelId.Position, Unreliable = true };

        private readonly Entity m_AttachedEntity;
        private readonly List<MsInInterval> m_MsInIntervals = new List<MsInInterval>();
        private readonly Dictionary<Entity, long> m_PositionTimestamps = new Dictionary<Entity, long>();

        private readonly object m_QueueLock = new object();
        private readonly List<Entity> m_QueuedPositionUpdates = new List<Entity>();
        private readonly Thread m_DequeueThread;
        private readonly List<Entity> m_RemoveList = new List<Entity>();
        private bool m_Dequeueing = true;
        private const int DequeueIntervalInMs = 15;

        static PositionFilter() {
            Time.Start();
        }

        public PositionFilter(Entity entity) {
            m_AttachedEntity = entity;
            UpdateIntervals();
            m_DequeueThread = new Thread(DequeueTask);
            m_DequeueThread.Start();
        }

        public void UpdateIntervals() {
            m_MsInIntervals.Clear();
            AddDefaultIntervals();
            AddWeaponInterval();
        }

        private void DequeueTask() {
            while (m_Dequeueing) {
                lock (m_QueueLock) {
                    foreach (var entity in m_QueuedPositionUpdates) {
                        var curTime = Time.ElapsedMilliseconds;
                        if (m_PositionTimestamps[entity] + GetUpdateInterval(entity) <= curTime) {
                            UpdateClientPosition(entity);
                            m_RemoveList.Add(entity);
                        }
                    }
                    while (m_RemoveList.Count > 0) {
                        m_QueuedPositionUpdates.Remove(m_RemoveList[0]);
                        m_RemoveList.RemoveAt(0);
                    }
                }
                Thread.Sleep(DequeueIntervalInMs);
            }
        }

        public void OnPositionUpdate(Entity entity) {
            lock (m_QueueLock) {
                // If no information about past update to this entity.
                if (!m_PositionTimestamps.ContainsKey(entity)) {
                    UpdateClientPosition(entity);
                    return;
                }

                if (!m_QueuedPositionUpdates.Contains(entity)) {
                    m_QueuedPositionUpdates.Add(entity);
                }
            }
        }

        private int GetUpdateInterval(Entity entity) {
            var distance = Vector.Distance(entity.Position, m_AttachedEntity.Position);
            var interval = int.MaxValue;
            foreach (var msInIntervals in m_MsInIntervals) {
                if (!msInIntervals.IsInInterval(distance)) { continue; }
                if (msInIntervals.MilliSeconds > interval) { continue; }
                interval = msInIntervals.MilliSeconds;
            }
            return interval;
        }

        private long TimeSinceLastUpdate(Entity entity) {
            if (!m_PositionTimestamps.ContainsKey(entity)) { return -1L; }
            return Time.ElapsedMilliseconds - m_PositionTimestamps[entity];
        }

        private void AddDefaultIntervals() {
            m_MsInIntervals.Add(new MsInInterval(0F, float.PositiveInfinity, 1000));
            m_MsInIntervals.Add(new MsInInterval(0F, 10F, 200));
        }

        private void AddWeaponInterval() {
            var weapon = (IntAttribute)m_AttachedEntity.GetAttribute(AttributeCode.Weapon);
            if (weapon == null) { return; }

            switch ((WeaponCode)weapon.GetValue()) {
                case WeaponCode.Axe:
                    m_MsInIntervals.Add(new MsInInterval(0F, AxeAutoAttackRequest.ATTACKDISTANCE + 0.5F, 0));
                    break;
                case WeaponCode.Bow:
                    m_MsInIntervals.Add(new MsInInterval(0F, BowAutoAttackRequest.ATTACKDISTANCE + 0.5F, 0));
                    break;
            }
        }

        private void UpdateClientPosition(Entity entity) {
            EventMessage.CounterEventReceive.Increment();
            var moveEvent = new MoveEvent(entity.Name, entity.Position);
            IEventData eventData = new EventData((byte)EventCode.Move, moveEvent);
            m_AttachedEntity.SendEvent(eventData, PositionSendParameters);
            AddTimeStamp(entity);
        }

        private void AddTimeStamp(Entity entity) {
            m_PositionTimestamps[entity] = Time.ElapsedMilliseconds;
        }
    }
}