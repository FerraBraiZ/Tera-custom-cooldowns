﻿using System.Linq;
using TCC.Data;
using TCC.Parsing.Messages;
using TCC.ViewModels;

namespace TCC.ClassSpecific
{
    public class WarriorAbnormalityTracker : ClassAbnormalityTracker
    {
        private static readonly uint[] GambleIDs = { 100800, 100801, 100802, 100803 };
        private static readonly uint[] AstanceIDs = { 100100, 100101, 100102, 100103 };
        private static readonly uint[] DstanceIDs = { 100200, 100201, 100202, 100203 };
        private static readonly uint[] TraverseCutIDs = { 101300, 101301 };
        private static readonly uint[] BladeWaltzIDs = { 104100 };

        public override void CheckAbnormality(S_ABNORMALITY_BEGIN p)
        {
            if (p.TargetId != SessionManager.CurrentPlayer.EntityId) return;
            CheckAssaultStance(p);
            CheckDefensiveStance(p);
            CheckDeadlyGamble(p);
            CheckTraverseCut(p);
            CheckBladeWaltz(p);
        }
        public override void CheckAbnormality(S_ABNORMALITY_REFRESH p)
        {
            if (p.TargetId != SessionManager.CurrentPlayer.EntityId) return;
            CheckAssaultStance(p);
            CheckDefensiveStance(p);
            CheckDeadlyGamble(p);
            CheckTraverseCut(p);
            //CheckTempestAura(p);
        }
        public override void CheckAbnormality(S_ABNORMALITY_END p)
        {
            if (p.TargetId != SessionManager.CurrentPlayer.EntityId) return;
            CheckTraverseCut(p);
            CheckDefensiveStance(p);
            CheckAssaultStance(p);
            CheckDeadlyGamble(p);
            //CheckTempestAura(p);
        }

        private static void CheckAssaultStance(S_ABNORMALITY_BEGIN p)
        {
            if (!AstanceIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).Stance.CurrentStance = WarriorStance.Assault;
        }
        private static void CheckAssaultStance(S_ABNORMALITY_REFRESH p)
        {
            if (!AstanceIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).Stance.CurrentStance = WarriorStance.Assault;
        }
        private static void CheckAssaultStance(S_ABNORMALITY_END p)
        {
            if (!AstanceIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).Stance.CurrentStance = WarriorStance.None;
        }

        private static void CheckDefensiveStance(S_ABNORMALITY_BEGIN p)
        {
            if (!DstanceIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).Stance.CurrentStance = WarriorStance.Defensive;
        }
        private static void CheckDefensiveStance(S_ABNORMALITY_REFRESH p)
        {
            if (!DstanceIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).Stance.CurrentStance = WarriorStance.Defensive;
        }
        private static void CheckDefensiveStance(S_ABNORMALITY_END p)
        {
            if (!DstanceIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).Stance.CurrentStance = WarriorStance.None;
        }

        private static void CheckDeadlyGamble(S_ABNORMALITY_BEGIN p)
        {
            if (!GambleIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).DeadlyGamble.Buff.Start(p.Duration);
        }
        private static void CheckDeadlyGamble(S_ABNORMALITY_REFRESH p)
        {
            if (!GambleIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).DeadlyGamble.Buff.Refresh(p.Duration);
        }
        private static void CheckDeadlyGamble(S_ABNORMALITY_END p)
        {
            if (!GambleIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).DeadlyGamble.Buff.Refresh(0);
        }

        private static void CheckBladeWaltz(S_ABNORMALITY_BEGIN p)
        {
            if (!BladeWaltzIDs.Contains(p.AbnormalityId)) return;
            if (!SessionManager.SkillsDatabase.TryGetSkillByIconName("icon_skills.doublesworddance_tex", SessionManager.CurrentPlayer.Class, out var sk)) return;
            CooldownWindowViewModel.Instance.AddOrRefresh(new SkillCooldown(sk, p.Duration, CooldownType.Skill, CooldownWindowViewModel.Instance.GetDispatcher(),true, true));

        }

        private static void CheckTraverseCut(S_ABNORMALITY_BEGIN p)
        {
            if (!TraverseCutIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TraverseCut.Val = p.Stacks;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TraverseCut.ToZero(p.Duration);
        }
        private static void CheckTraverseCut(S_ABNORMALITY_REFRESH p)
        {
            if (!TraverseCutIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TraverseCut.Val = p.Stacks;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TraverseCut.ToZero(p.Duration);
        }
        private static void CheckTraverseCut(S_ABNORMALITY_END p)
        {
            if (!TraverseCutIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TraverseCut.Val = 0;
        }
    }
}
/*
        private static readonly uint[] TempestAuraIDs = { 103000, 103102, 103120, 103131 };
        private static readonly uint[] ShadowTempestIDs = { 103104, 103130 };

        private static void CheckTempestAura(S_ABNORMALITY_BEGIN p)
        {
            if (!TempestAuraIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TempestAura.Val = p.Stacks;
        }
        private static void CheckTempestAura(S_ABNORMALITY_REFRESH p)
        {
            if (!TempestAuraIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TempestAura.Val = p.Stacks;
        }
        private static void CheckTempestAura(S_ABNORMALITY_END p)
        {
            if (!TempestAuraIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TempestAura.Val = 0;
        }
        private static void CheckShadowTempest(S_ABNORMALITY_BEGIN p)
        {
            if (!ShadowTempestIDs.Contains(p.AbnormalityId)) return;
            ((WarriorBarManager)ClassWindowViewModel.Instance.CurrentManager).TempestAura.ToZero(p.Duration);
        }
*/
