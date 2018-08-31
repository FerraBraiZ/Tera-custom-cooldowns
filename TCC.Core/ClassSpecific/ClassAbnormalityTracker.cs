﻿using System;
using System.Collections.Generic;
using TCC.Parsing.Messages;

namespace TCC.ClassSpecific
{
    public class ClassAbnormalityTracker
    {
        protected static readonly List<ulong> MarkedTargets = new List<ulong>();
        public static event Action<ulong> MarkingRefreshed;
        public static event Action MarkingExpired;

        public virtual void CheckAbnormality(S_ABNORMALITY_BEGIN p) { }
        public virtual void CheckAbnormality(S_ABNORMALITY_REFRESH p) { }
        public virtual void CheckAbnormality(S_ABNORMALITY_END p) { }

        protected static void InvokeMarkingExpired() => MarkingExpired?.Invoke();
        protected static void InvokeMarkingRefreshed(ulong duration) => MarkingRefreshed?.Invoke(duration);
        public static void CheckMarkingOnDespawn(ulong target)
        {
            if (!MarkedTargets.Contains(target)) return;
            MarkedTargets.Remove(target);
            if (MarkedTargets.Count == 0) InvokeMarkingExpired();
        }

        private static void ClearMarkedTargets()
        {
            App.BaseDispatcher.Invoke(() => MarkedTargets.Clear());
        }

        protected ClassAbnormalityTracker()
        {
            ClearMarkedTargets();
        }
    }
}