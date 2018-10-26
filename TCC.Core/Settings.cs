﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TCC.Data;
using TCC.Parsing;
using Key = System.Windows.Forms.Keys;

namespace TCC
{
    public static class Settings
    {
        public static double ScreenW => SystemParameters.VirtualScreenWidth;
        public static double ScreenH => SystemParameters.VirtualScreenHeight;

        public static WindowSettings GroupWindowSettings { get; set; } = new WindowSettings(0, 0, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, false, false, null, nameof(GroupWindowSettings));
        public static WindowSettings CooldownWindowSettings { get; set; } = new WindowSettings(.4, .7, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, true, false, null, nameof(CooldownWindowSettings));
        public static WindowSettings BossWindowSettings { get; set; } = new WindowSettings(.4, 0, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, false, false, null, nameof(BossWindowSettings));
        public static WindowSettings BuffWindowSettings { get; set; } = new WindowSettings(1, .7, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, true, false, null, nameof(BuffWindowSettings));
        public static WindowSettings CharacterWindowSettings { get; set; } = new WindowSettings(.4, 1, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, false, false, null, nameof(CharacterWindowSettings));
        public static WindowSettings ClassWindowSettings { get; set; } = new WindowSettings(.25, .6, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, true, false, null, nameof(ClassWindowSettings));
        public static WindowSettings FlightGaugeWindowSettings { get; set; } = new WindowSettings(0, 0, 0, 0, true, ClickThruMode.Always, 1, false, 1, false, true, false);
        public static WindowSettings FloatingButtonSettings { get; set; } = new WindowSettings(0, 0, 0, 0, true, ClickThruMode.Never, 1, false, 1, false, true, true);
        public static WindowSettings CivilUnrestWindowSettings { get; set; } = new WindowSettings(1, .45,0,0, true, ClickThruMode.Never,1,true,.5,false,true,false,null,nameof(CivilUnrestWindowSettings));

        public static SynchronizedObservableCollection<ChatWindowSettings> ChatWindowsSettings { get; set; } = new SynchronizedObservableCollection<ChatWindowSettings>();

        // Group window
        public static bool IgnoreMeInGroupWindow { get; set; }
        public static bool IgnoreGroupBuffs { get; set; }
        public static bool IgnoreGroupDebuffs { get; set; }
        public static bool DisablePartyMP { get; set; }
        public static bool DisablePartyHP { get; set; }
        public static bool DisablePartyAbnormals { get; set; } 
        public static bool ShowOnlyAggroStacks { get; set; } = true;
        public static uint GroupSizeThreshold { get; set; } = 7;
        public static bool ShowMembersLaurels { get; set; }
        public static bool ShowAllGroupAbnormalities { get; set; }
        public static bool ShowGroupWindowDetails { get; set; } = true;
        public static bool ShowAwakenIcon { get; set; } = true;
        public static Dictionary<Class, List<uint>> GroupAbnormals { get; } = new Dictionary<Class, List<uint>>()
        {
            {0, new List<uint>()},
            {(Class)1, new List<uint>()},
            {(Class)2, new List<uint>()},
            {(Class)3, new List<uint>()},
            {(Class)4, new List<uint>()},
            {(Class)5, new List<uint>()},
            {(Class)6, new List<uint>()},
            {(Class)7, new List<uint>()},
            {(Class)8, new List<uint>()},
            {(Class)9, new List<uint>()},
            {(Class)10, new List<uint>()},
            {(Class)11, new List<uint>()},
            {(Class)12, new List<uint>()},
            {(Class)255, new List<uint>()},
        };
        // Buff window
        public static FlowDirection BuffsDirection { get; set; } = FlowDirection.RightToLeft;
        // Cooldown window
        public static CooldownBarMode CooldownBarMode { get; set; } = CooldownBarMode.Fixed;
        public static bool ShowItemsCooldown { get; set; } = true;

        // Boss window
        public static bool ShowOnlyBosses { get; set; }
        public static EnrageLabelMode EnrageLabelMode { get; set; } = EnrageLabelMode.Remaining;
        public static bool AccurateHp { get; set; } = true;

        // Chat window
        public static bool ChatEnabled
        {
            get => ChatWindowsSettings.Count > 0 ? ChatWindowsSettings[0].Enabled : _chatEnabled;
            set
            {
                if (ChatWindowsSettings.Count > 0)
                {
                    if (ChatWindowsSettings[0].Enabled == value) return;
                    ChatWindowsSettings.ToList().ForEach(x => x.Enabled = value);
                }
                else
                {
                    _chatEnabled = value;
                }
            }
        }
        public static int MaxMessages { get; set; } = 500;
        public static int SpamThreshold { get; set; } = 2;
        public static bool ShowChannel { get; set; } = true;
        public static bool ShowTimestamp { get; set; } = true;
        public static double ChatWindowOpacity { get; set; } = 0.4;
        public static int FontSize { get; set; } = 15;
        public static bool ChatFadeOut { get; set; } = true;
        public static bool AnimateChatMessages { get; set; }
        public static ClickThruMode ChatClickThruMode
        {
            get
            {
                if (ChatWindowsSettings.Count > 0) return ChatWindowsSettings[0].ClickThruMode;
                return _chatClickThruMode;
            }

            set
            {
                if (ChatWindowsSettings.Count > 0)
                {
                    if (ChatWindowsSettings[0].ClickThruMode == value) return;
                    ChatWindowsSettings.ToList().ForEach(x => x.ClickThruMode = value);
                }
                else
                {
                    _chatClickThruMode = value;
                }
            }
        }
        // Character window
        public static bool CharacterWindowCompactMode { get; set; } = true;
        // Class window
        public static bool WarriorShowTraverseCut { get; set; } = true;
        public static bool WarriorShowEdge { get; set; } = true;
        public static WarriorEdgeMode WarriorEdgeMode { get; set; } = WarriorEdgeMode.Bar;
        public static bool SorcererReplacesElementsInCharWindow { get; set; } = true;


        // Misc
        public static DateTime LastRun { get; set; } = DateTime.MinValue;
        public static string LastRegion
        {
            get => RegionOverride != "" ? RegionOverride : _lastRegion;
            set => _lastRegion = value;
        }
        public static string Webhook { get; set; } = "";
        public static string WebhookMessage { get; set; } = "@here Guild BAM will spawn soon!";
        public static string TwitchName { get; set; } = "";
        public static string TwitchToken { get; set; } = "";
        public static string TwitchChannelName { get; set; } = "";
        public static bool LfgEnabled { get; set; } = true;
        public static bool ShowTradeLfg { get; set; } = true;
        public static bool StatSent { get; set; }
        public static bool ShowFlightEnergy { get; set; } = true;
        public static bool UseHotkeys { get; set; } = true;
        public static HotKey LfgHotkey { get; } = new HotKey(Key.Y, ModifierKeys.Control);
        public static HotKey InfoWindowHotkey { get; } = new HotKey(Key.I, ModifierKeys.Control);
        public static HotKey SettingsHotkey { get; } = new HotKey(Key.O, ModifierKeys.Control);
        public static HotKey ShowAllHotkey { get; } = new HotKey(Key.NumPad5, ModifierKeys.Control);
        public static HotKey LootSettingsHotkey { get; } = new HotKey(Key.L, ModifierKeys.Control);
        public static string RegionOverride { get; set; } = "";
        public static double FlightGaugeRotation { get; set; }
        public static bool FlipFlightGauge { get; set; }
        public static bool HideHandles { get; set; }
        public static bool HighPriority { get; set; }
        public static bool ForceSoftwareRendering { get; set; }
        public static ControlShape AbnormalityShape { get; set; }
        public static ControlShape SkillShape { get; set; }
        public static bool Winpcap { get; set; } = true;

        private static string _lastRegion = "";
        private static bool _chatEnabled;
        private static ClickThruMode _chatClickThruMode;

    }
}
