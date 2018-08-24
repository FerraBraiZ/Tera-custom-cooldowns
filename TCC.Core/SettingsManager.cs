﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using TCC.Data;
using TCC.ViewModels;
using TCC.Windows;
using MessageBoxImage = TCC.Data.MessageBoxImage;
using ModifierKeys = TCC.Tera.Data.HotkeysData.ModifierKeys;
using Key = System.Windows.Forms.Keys;
using Point = System.Windows.Point;
using Size = System.Drawing.Size;

namespace TCC
{
    public static class SettingsManager
    {
        public static double ScreenW => SystemParameters.VirtualScreenWidth;
        public static double ScreenH => SystemParameters.VirtualScreenHeight;
        private static XDocument _settingsDoc;
        public static WindowSettings GroupWindowSettings = new WindowSettings(0, 0, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, false, false);
        public static WindowSettings CooldownWindowSettings = new WindowSettings(.4, .7, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, true, false);
        public static WindowSettings BossWindowSettings = new WindowSettings(.4, 0, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, false, false);
        public static WindowSettings BuffWindowSettings = new WindowSettings(1, .7, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, true, false);
        public static WindowSettings CharacterWindowSettings = new WindowSettings(.4, 1, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, false, false);
        public static WindowSettings ClassWindowSettings = new WindowSettings(.25, .6, 0, 0, true, ClickThruMode.Never, 1, true, .5, false, true, false);
        public static WindowSettings FlightGaugeWindowSettings = new WindowSettings(0, 0, 0, 0, true, ClickThruMode.Always, 1, false, 1, false, true, false);
        public static WindowSettings FloatingButtonSettings = new WindowSettings(0, 0, 0, 0, true, ClickThruMode.Never, 1, false, 1, false, true, true);

        public static SynchronizedObservableCollection<ChatWindowSettings> ChatWindowsSettings = new SynchronizedObservableCollection<ChatWindowSettings>();

        public static bool IgnoreMeInGroupWindow { get; set; }
        public static bool IgnoreGroupBuffs { get; set; }
        public static bool IgnoreGroupDebuffs { get; set; }
        public static FlowDirection BuffsDirection { get; set; } = FlowDirection.RightToLeft;
        public static CooldownBarMode CooldownBarMode { get; set; } = CooldownBarMode.Fixed;
        public static int MaxMessages { get; set; } = 500;
        public static int SpamThreshold { get; set; } = 2;
        public static bool ShowChannel { get; set; } = true;
        public static bool ShowTimestamp { get; set; } = true;
        public static bool ShowOnlyBosses { get; set; } = false;
        public static bool DisablePartyMP { get; set; } = false;
        public static bool DisablePartyHP { get; set; } = false;
        public static bool ShowOnlyAggroStacks { get; set; } = true;
        public static bool DisablePartyAbnormals { get; set; } = false;
        public static double ChatWindowOpacity { get; set; } = 0.4;
        public static DateTime LastRun { get; set; } = DateTime.MinValue;

        public static string LastRegion
        {
            get
            {
                if (RegionOverride != "") return RegionOverride;
                return _lastRegion;
            }
            set => _lastRegion = value;
        }

        public static string Webhook { get; set; } = "";
        //public static List<ChatChannelOnOff> EnabledChatChannels { get; set; } = Utils.GetEnabledChannelsList();
        public static string WebhookMessage { get; set; } = "@here Guild BAM will spawn soon!";
        public static int FontSize { get; set; } = 15;
        public static string TwitchName { get; set; } = "";
        public static string TwitchToken { get; set; } = "";
        public static string TwitchChannelName { get; set; } = "";
        public static bool ChatFadeOut { get; set; } = true;
        public static bool ShowTradeLfg { get; set; } = true;

        public static readonly Dictionary<Class, List<uint>> GroupAbnormals = new Dictionary<Class, List<uint>>()
        {
            {(Class)0, new List<uint>()},
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


        public static uint GroupSizeThreshold = 7;
        public static EnrageLabelMode EnrageLabelMode { get; set; } = EnrageLabelMode.Remaining;
        public static bool ShowItemsCooldown { get; set; } = true;
        public static bool ShowMembersLaurels { get; set; } = false;
        public static bool AnimateChatMessages { get; set; } = false;

        public static bool ChatEnabled
        {
            get
            {
                if (ChatWindowsSettings.Count > 0) return ChatWindowsSettings[0].Enabled;
                return _chatEnabled;
            }
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

        public static bool ShowAllGroupAbnormalities { get; set; } = false;

        public static bool StatSent { get; set; } = false;
        public static bool ShowFlightEnergy { get; set; } = true;
        public static bool LfgEnabled { get; set; } = true;
        public static bool ShowGroupWindowDetails { get; set; } = true;
        public static bool UseHotkeys { get; set; } = true;
        public static bool AccurateHp { get; set; } = true;
        public static HotKey LfgHotkey { get; set; } = new HotKey(Key.Y, ModifierKeys.Control);
        public static HotKey InfoWindowHotkey { get; set; } = new HotKey(Key.I, ModifierKeys.Control);
        public static HotKey SettingsHotkey { get; set; } = new HotKey(Key.O, ModifierKeys.Control);
        public static HotKey ShowAllHotkey { get; set; } = new HotKey(Key.NumPad5, ModifierKeys.Control);
        public static HotKey LootSettingsHotkey { get; set; } = new HotKey(Key.L, ModifierKeys.Control);
        public static string RegionOverride { get; set; } = "";
        public static bool ShowAwakenIcon { get; set; } = true;
        public static bool CharacterWindowCompactMode { get; set; } = true;
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

        public static bool WarriorShowTraverseCut { get; set; } = true;
        public static bool WarriorShowEdge { get; set; } = true;
        public static WarriorEdgeMode WarriorEdgeMode { get; set; } = WarriorEdgeMode.Bar;
        public static double FlightGaugeRotation { get; set; }
        public static bool FlipFlightGauge { get; set; }


        public static void LoadWindowSettings()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml")) return;
            try
            {
                _settingsDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml");

                foreach (var ws in _settingsDoc.Descendants().Where(x => x.Name == "WindowSetting"))
                {
                    var name = ws.Attribute("Name")?.Value;
                    if (name == null) return;
                    if (name == "BossWindow") BossWindowSettings = ParseWindowSettings(ws);
                    else if (name == "CharacterWindow") CharacterWindowSettings = ParseWindowSettings(ws);
                    else if (name == "CooldownWindow") CooldownWindowSettings = ParseWindowSettings(ws);
                    else if (name == "BuffWindow") BuffWindowSettings = ParseWindowSettings(ws);
                    else if (name == "GroupWindow") GroupWindowSettings = ParseWindowSettings(ws);
                    else if (name == "ClassWindow") ClassWindowSettings = ParseWindowSettings(ws);
                    else if (name == "FlightGaugeWindow") FlightGaugeWindowSettings = ParseWindowSettings(ws);
                    else if (name == "FloatingButton") FloatingButtonSettings = ParseWindowSettings(ws);
                    //add window here
                }

                if (_settingsDoc.Descendants().Count(x => x.Name == "ChatWindow") > 0)
                {
                    _settingsDoc.Descendants().Where(x => x.Name == "ChatWindow").ToList().ForEach(s =>
                    {
                        ChatWindowsSettings.Add(ParseChatWindowSettings(s));
                    });
                }
            }
            catch (XmlException)
            {
                var res = TccMessageBox.Show("TCC",
                    "Cannot load settings file. Do you want TCC to delete it and recreate a default file?",
                    MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes) File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml");
                LoadWindowSettings();
            }
        }



        private static ChatWindowSettings ParseChatWindowSettings(XContainer s)
        {
            var ws = s.Descendants().FirstOrDefault(x => x.Name == "WindowSetting");
            var ts = s.Descendants().FirstOrDefault(x => x.Name == "Tabs");
            var lfg = ws.Attribute(nameof(ChatWindowSettings.LfgOn));
            var op = ws.Attribute(nameof(ChatWindowSettings.BackgroundOpacity));

            var sett = ParseWindowSettings(ws);
            var tabs = ParseTabsSettings(ts);

            return new ChatWindowSettings(sett.X, sett.Y, sett.H, sett.W,
                                          sett.Visible, sett.ClickThruMode,
                                          sett.Scale, sett.AutoDim, sett.DimOpacity,
                                          sett.ShowAlways, /*sett.AllowTransparency,*/
                                          sett.Enabled, sett.AllowOffScreen)
            {
                Tabs = tabs,
                LfgOn = lfg == null || bool.Parse(lfg.Value),
                BackgroundOpacity = op != null ? double.Parse(op.Value, CultureInfo.InvariantCulture) : 0.3
            };
        }

        private static List<Tab> ParseTabsSettings(XElement elem)
        {
            var result = new List<Tab>();
            if (elem != null)
            {
                foreach (var t in elem.Descendants().Where(x => x.Name == "Tab"))
                {
                    var tabName = t.Attribute("name").Value;
                    var channels = new List<ChatChannel>();
                    var exChannels = new List<ChatChannel>();
                    var authors = new List<string>();
                    var exAuthors = new List<string>();
                    foreach (var chElement in t.Descendants().Where(x => x.Name == "Channel"))
                    {
                        channels.Add((ChatChannel)Enum.Parse(typeof(ChatChannel), chElement.Attribute("value").Value));
                    }
                    foreach (var chElement in t.Descendants().Where(x => x.Name == "ExcludedChannel"))
                    {
                        exChannels.Add((ChatChannel)Enum.Parse(typeof(ChatChannel), chElement.Attribute("value").Value));
                    }
                    foreach (var authElement in t.Descendants().Where(x => x.Name == "Author"))
                    {
                        authors.Add(authElement.Attribute("value").Value);
                    }
                    foreach (var authElement in t.Descendants().Where(x => x.Name == "ExcludedAuthor"))
                    {
                        exAuthors.Add(authElement.Attribute("value").Value);
                    }

                    result.Add(new Tab(tabName, channels.ToArray(), exChannels.ToArray(), authors.ToArray(), exAuthors.ToArray()));
                }
            }
            return result;
        }


        public static void LoadSettings()
        {
            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml")) return;
                _settingsDoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml");

                //TODO: iterate thru attributes and just check names (like parsewindowsettings)
                var b = _settingsDoc.Descendants("OtherSettings").FirstOrDefault();
                if (b == null) return;
                b.Attributes().ToList().ForEach(attr =>
                {
                    if (attr.Name == nameof(IgnoreMeInGroupWindow)) IgnoreMeInGroupWindow = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(IgnoreGroupBuffs)) IgnoreGroupBuffs = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(IgnoreGroupDebuffs)) IgnoreGroupDebuffs = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(BuffsDirection)) BuffsDirection = (FlowDirection)Enum.Parse(typeof(FlowDirection), attr.Value);
                    else if (attr.Name == nameof(CooldownBarMode)) CooldownBarMode = (CooldownBarMode)Enum.Parse(typeof(CooldownBarMode), attr.Value);
                    else if (attr.Name == nameof(CooldownBarMode)) CooldownBarMode = (CooldownBarMode)Enum.Parse(typeof(CooldownBarMode), attr.Value);
                    else if (attr.Name == nameof(EnrageLabelMode)) EnrageLabelMode = (EnrageLabelMode)Enum.Parse(typeof(EnrageLabelMode), attr.Value);
                    else if (attr.Name == nameof(ChatClickThruMode)) ChatClickThruMode = (ClickThruMode)Enum.Parse(typeof(ClickThruMode), attr.Value);
                    else if (attr.Name == nameof(WarriorEdgeMode)) WarriorEdgeMode = (WarriorEdgeMode)Enum.Parse(typeof(WarriorEdgeMode), attr.Value);
                    else if (attr.Name == nameof(MaxMessages)) MaxMessages = int.Parse(attr.Value);
                    else if (attr.Name == nameof(SpamThreshold)) SpamThreshold = int.Parse(attr.Value);
                    else if (attr.Name == nameof(FontSize)) FontSize = int.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowChannel)) ShowChannel = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowTimestamp)) ShowTimestamp = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowOnlyBosses)) ShowOnlyBosses = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(DisablePartyHP)) DisablePartyHP = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(DisablePartyMP)) DisablePartyMP = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowOnlyAggroStacks)) ShowOnlyAggroStacks = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(DisablePartyAbnormals)) DisablePartyAbnormals = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ChatFadeOut)) ChatFadeOut = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowItemsCooldown)) ShowItemsCooldown = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowMembersLaurels)) ShowMembersLaurels = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(AnimateChatMessages)) AnimateChatMessages = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(StatSent)) StatSent = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowFlightEnergy)) ShowFlightEnergy = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(LfgEnabled)) LfgEnabled = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowGroupWindowDetails)) ShowGroupWindowDetails = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(UseHotkeys)) UseHotkeys = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ChatEnabled)) ChatEnabled = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowTradeLfg)) ShowTradeLfg = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowAwakenIcon)) ShowAwakenIcon = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(AccurateHp)) AccurateHp = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(WarriorShowEdge)) WarriorShowEdge = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(FlipFlightGauge)) FlipFlightGauge = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(WarriorShowTraverseCut)) WarriorShowTraverseCut = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(CharacterWindowCompactMode)) CharacterWindowCompactMode = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(ShowAllGroupAbnormalities)) ShowAllGroupAbnormalities = bool.Parse(attr.Value);
                    else if (attr.Name == nameof(RegionOverride)) RegionOverride = attr.Value;
                    else if (attr.Name == nameof(LastRegion)) LastRegion = attr.Value;
                    else if (attr.Name == nameof(Webhook)) Webhook = attr.Value;
                    else if (attr.Name == nameof(WebhookMessage)) WebhookMessage = attr.Value;
                    else if (attr.Name == nameof(ChatWindowOpacity)) ChatWindowOpacity = double.Parse(attr.Value, CultureInfo.InvariantCulture);
                    else if (attr.Name == nameof(FlightGaugeRotation)) FlightGaugeRotation = double.Parse(attr.Value, CultureInfo.InvariantCulture);
                    else if (attr.Name == nameof(LastRun)) LastRun = DateTime.Parse(attr.Value);
                    else if (attr.Name == nameof(TwitchName)) TwitchName = attr.Value;
                    else if (attr.Name == nameof(TwitchToken)) TwitchToken = attr.Value;
                    else if (attr.Name == nameof(TwitchChannelName)) TwitchChannelName = attr.Value;
                    else if (attr.Name == nameof(GroupSizeThreshold)) GroupSizeThreshold = uint.Parse(attr.Value);
                    //add settings here
                });


                //try
                //{
                //    ParseChannelsSettings(_settingsDoc.Descendants().FirstOrDefault(x => x.Name == nameof(EnabledChatChannels)));
                //}
                //catch (Exception) { }

                try
                {
                    ParseGroupAbnormalSettings(_settingsDoc.Descendants().FirstOrDefault(x => x.Name == nameof(GroupAbnormals)));
                }
                catch
                {
                    CommonDefault.ForEach(x => GroupAbnormals[Class.Common].Add(x));
                    PriestDefault.ForEach(x => GroupAbnormals[Class.Priest].Add(x));
                    MysticDefault.ForEach(x => GroupAbnormals[Class.Mystic].Add(x));
                }
            }
            catch (XmlException)
            {
                var res = TccMessageBox.Show("TCC",
                    "Cannot load settings file. Do you want TCC to delete it and recreate a default file?",
                    MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes) File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml");
                LoadSettings();
            }
        }
        public static void SaveSettings()
        {
            var xSettings = new XElement("Settings",
                new XElement("WindowSettings",
                    BossWindowSettings.ToXElement("BossWindow"),
                    BuffWindowSettings.ToXElement("BuffWindow"),
                    CharacterWindowSettings.ToXElement("CharacterWindow"),
                    CooldownWindowSettings.ToXElement("CooldownWindow"),
                    GroupWindowSettings.ToXElement("GroupWindow"),
                    ClassWindowSettings.ToXElement("ClassWindow"),
                    BuildChatWindowSettings("ChatWindows"),
                    FlightGaugeWindowSettings.ToXElement("FlightGaugeWindow"),
                    FloatingButtonSettings.ToXElement("FloatingButton")
                    //ChatWindowSettings.ToXElement("ChatWindow")
                    //add window here
                    ),
                new XElement("OtherSettings",
                new XAttribute(nameof(IgnoreMeInGroupWindow), IgnoreMeInGroupWindow),
                new XAttribute(nameof(IgnoreGroupBuffs), IgnoreGroupBuffs),
                new XAttribute(nameof(IgnoreGroupDebuffs), IgnoreGroupDebuffs),
                new XAttribute(nameof(BuffsDirection), BuffsDirection),
                new XAttribute(nameof(CooldownBarMode), CooldownBarMode),
                new XAttribute(nameof(MaxMessages), MaxMessages),
                new XAttribute(nameof(SpamThreshold), SpamThreshold),
                new XAttribute(nameof(FontSize), FontSize),
                new XAttribute(nameof(ShowChannel), ShowChannel),
                new XAttribute(nameof(ShowTimestamp), ShowTimestamp),
                new XAttribute(nameof(ShowOnlyBosses), ShowOnlyBosses),
                new XAttribute(nameof(DisablePartyMP), DisablePartyMP),
                new XAttribute(nameof(DisablePartyHP), DisablePartyHP),
                new XAttribute(nameof(DisablePartyAbnormals), DisablePartyAbnormals),
                new XAttribute(nameof(ShowOnlyAggroStacks), ShowOnlyAggroStacks),
                new XAttribute(nameof(ChatFadeOut), ChatFadeOut),
                new XAttribute(nameof(ChatWindowOpacity), ChatWindowOpacity),
                new XAttribute(nameof(LastRun), DateTime.Now),
                new XAttribute(nameof(LastRegion), LastRegion),
                new XAttribute(nameof(Webhook), Webhook),
                new XAttribute(nameof(WebhookMessage), WebhookMessage),
                new XAttribute(nameof(TwitchName), TwitchName),
                new XAttribute(nameof(TwitchToken), TwitchToken),
                new XAttribute(nameof(TwitchChannelName), TwitchChannelName),
                new XAttribute(nameof(GroupSizeThreshold), GroupSizeThreshold),
                new XAttribute(nameof(EnrageLabelMode), EnrageLabelMode),
                new XAttribute(nameof(ShowMembersLaurels), ShowMembersLaurels),
                new XAttribute(nameof(AnimateChatMessages), AnimateChatMessages),
                new XAttribute(nameof(ShowItemsCooldown), ShowItemsCooldown),
                new XAttribute(nameof(StatSent), StatSent),
                new XAttribute(nameof(ShowFlightEnergy), ShowFlightEnergy),
                new XAttribute(nameof(LfgEnabled), LfgEnabled),
                new XAttribute(nameof(ShowGroupWindowDetails), ShowGroupWindowDetails),
                new XAttribute(nameof(UseHotkeys), UseHotkeys),
                new XAttribute(nameof(ChatEnabled), ChatEnabled),
                new XAttribute(nameof(ShowTradeLfg), ShowTradeLfg),
                new XAttribute(nameof(ShowAwakenIcon), ShowAwakenIcon),
                new XAttribute(nameof(RegionOverride), RegionOverride),
                new XAttribute(nameof(ChatClickThruMode), ChatClickThruMode),
                new XAttribute(nameof(AccurateHp), AccurateHp),
                new XAttribute(nameof(CharacterWindowCompactMode), CharacterWindowCompactMode),
                new XAttribute(nameof(WarriorShowTraverseCut), WarriorShowTraverseCut),
                new XAttribute(nameof(WarriorShowEdge), WarriorShowEdge),
                new XAttribute(nameof(WarriorEdgeMode), WarriorEdgeMode),
                new XAttribute(nameof(FlightGaugeRotation), FlightGaugeRotation),
                new XAttribute(nameof(FlipFlightGauge), FlipFlightGauge),
                new XAttribute(nameof(ShowAllGroupAbnormalities), ShowAllGroupAbnormalities)
                //add setting here
                ),
                //BuildChannelsXElement(),
                //BuildChatTabsXElement(),
                BuildGroupAbnormalsXElement()
            );
            SaveSettingsDoc(xSettings);
        }



        private static void SaveSettingsDoc(XElement doc)
        {
            if (!doc.HasElements) return;
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml")) File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml", AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml.bak", true);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory + @"/tcc-config.xml");
            }
            catch (Exception)
            {
                var res = TccMessageBox.Show("TCC", "Could not write settings data to tcc-config.xml. File is being used by another process. Try again?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes) SaveSettingsDoc(doc);
            }

        }
        private static WindowSettings ParseWindowSettings(XElement ws) //TODO: don't overwrite defaults
        {
            double x = 0, y = 0, w = 0, h = 0, scale = 1, dimOp = .3;
            var ctm = ClickThruMode.Never;
            bool vis = true, enabled = true, autoDim = true, allowOffscreen = false, alwaysVis = false;
            Dictionary<Class, Point> positions = null;

            try
            {
                x = double.Parse(ws.Attribute("X").Value, CultureInfo.InvariantCulture);
                if (x > 1) x = x / ScreenW;
            }
            catch (Exception) { }
            try
            {
                y = double.Parse(ws.Attribute("Y").Value, CultureInfo.InvariantCulture);
                if (y > 1) y = y / ScreenH;
            }
            catch (Exception) { }
            try
            {
                w = double.Parse(ws.Attribute("W").Value, CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            try
            {
                h = double.Parse(ws.Attribute("H").Value, CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            try
            {
                ctm = (ClickThruMode)Enum.Parse(typeof(ClickThruMode), ws.Attribute(nameof(ClickThruMode)).Value);
            }
            catch (Exception) { }
            try
            {
                //w.Visibility = (Visibility)Enum.Parse(typeof(Visibility), ws.Attribute("Visibility").Value);
                vis = bool.Parse(ws.Attribute("Visible").Value);
            }
            catch (Exception) { }

            try
            {
                scale = double.Parse(ws.Attribute("Scale").Value, CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            try
            {
                autoDim = bool.Parse(ws.Attribute("AutoDim").Value);
            }
            catch (Exception) { }
            try
            {
                dimOp = double.Parse(ws.Attribute("DimOpacity").Value, CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            try
            {
                alwaysVis = bool.Parse(ws.Attribute("ShowAlways").Value);
            }
            catch (Exception) { }
            try
            {
                allowOffscreen = Boolean.Parse(ws.Attribute("AllowOffScreen").Value);
            }
            catch (Exception) { }
            try
            {
                enabled = bool.Parse(ws.Attribute("Enabled").Value);
            }
            catch (Exception) { }

            try
            {
                var pss = ws.Descendants().FirstOrDefault(s => s.Name == "Positions");
                if(pss != null) positions = new Dictionary<Class, Point>();
                pss?.Descendants().Where(s => s.Name == "Position").ToList().ForEach(pos =>
                {
                    var cl = (Class)Enum.Parse(typeof(Class), pos.Attribute("class").Value);
                    var px = double.Parse(pos.Attribute("X")?.Value, CultureInfo.InvariantCulture);
                    var py = double.Parse(pos.Attribute("Y")?.Value, CultureInfo.InvariantCulture);
                    positions.Add(cl, new Point(px, py));
                });
            }
            catch (Exception)
            {
                positions = null;
            }
            return new WindowSettings(x, y, h, w, vis, ctm, scale, autoDim, dimOp, alwaysVis, enabled, allowOffscreen, positions);
        }
        //private static void ParseChannelsSettings(XElement xElement)
        //{
        //    foreach (var e in xElement.Descendants().Where(x => x.Name == "Channel"))
        //    {
        //        EnabledChatChannels.FirstOrDefault(x => x.Channel == (ChatChannel)Enum.Parse(typeof(ChatChannel), e.Attribute("name").Value)).Enabled = bool.Parse(e.Attribute("enabled").Value);
        //    }
        //}
        private static void ParseGroupAbnormalSettings(XElement el)
        {
            //GroupAbnormals = new Dictionary<Class, List<uint>>();
            foreach (var groupAbnormalList in GroupAbnormals)
            {
                groupAbnormalList.Value.Clear();
            }

            foreach (var abEl in el.Descendants().Where(x => x.Name == "Abnormals"))
            {
                var stringClass = abEl.Attribute("class").Value;
                var parsedClass = (Class)Enum.Parse(typeof(Class), stringClass);
                var abnormalities = abEl.Value.Split(',');
                var list = abnormalities.Length == 1 && abnormalities[0] == "" ? new List<uint>() : abnormalities.Select(uint.Parse).ToList();
                list.ForEach(abnormality => GroupAbnormals[parsedClass].Add(abnormality));
                //GroupAbnormals.Add(cl, l);
            }

            if (GroupAbnormals.Count != 0) return;
            CommonDefault.ForEach(x => GroupAbnormals[Class.Common].Add(x));
            PriestDefault.ForEach(x => GroupAbnormals[Class.Priest].Add(x));
            MysticDefault.ForEach(x => GroupAbnormals[Class.Mystic].Add(x));
        }

        private static readonly List<uint> CommonDefault = new List<uint> { 4000, 4001, 4010, 4011, 4020, 4021, 4030, 4031, 4600, 4610, 4611, 4613, 5000003, 4830, 4831, 4833, 4841, 4886, 4861, 4953, 4955, 7777015, 902, 910, 911, 912, 913, 916, 920, 921, 922, 999010000 };
        private static readonly List<uint> PriestDefault = new List<uint> { 201, 202, 805100, 805101, 805102, 98000109, 805600, 805601, 805602, 805603, 805604, 98000110, 800300, 800301, 800302, 800303, 800304, 801500, 801501, 801502, 801503, 98000107 };
        private static readonly List<uint> MysticDefault = new List<uint> { 27120, 700630, 700631, 601, 602, 603, 700330, 700230, 700231, 800132, 700233, 700730, 700731, 700100 };
        private static string _lastRegion = "";
        private static bool _chatEnabled = false;
        private static ClickThruMode _chatClickThruMode;

        public static XElement BuildChatTabsXElement(List<Tab> tabList)
        {
            var result = new XElement("Tabs");
            foreach (var tab in tabList)
            {
                var tabName = new XAttribute("name", tab.TabName);
                var tabElement = new XElement("Tab", tabName);
                foreach (var ch in tab.Channels)
                {
                    tabElement.Add(new XElement("Channel", new XAttribute("value", ch)));
                }
                foreach (var ch in tab.Authors)
                {
                    tabElement.Add(new XElement("Author", new XAttribute("value", ch)));
                }
                foreach (var ch in tab.ExcludedChannels)
                {
                    tabElement.Add(new XElement("ExcludedChannel", new XAttribute("value", ch)));
                }
                foreach (var ch in tab.ExcludedAuthors)
                {
                    tabElement.Add(new XElement("ExcludedAuthor", new XAttribute("value", ch)));
                }
                result.Add(tabElement);

            }
            return result;
        }
        //private static XElement BuildChannelsXElement()
        //{
        //    var result = new XElement(nameof(EnabledChatChannels));
        //    foreach (var c in EnabledChatChannels)
        //    {
        //        var name = new XAttribute("name", c.Channel.ToString());
        //        var val = new XAttribute("enabled", c.Enabled.ToString());
        //        var chElement = new XElement("Channel", name, val);
        //        result.Add(chElement);
        //    }
        //    return result;
        //}
        private static XElement BuildGroupAbnormalsXElement()
        {
            var result = new XElement(nameof(GroupAbnormals));
            foreach (var pair in GroupAbnormals)
            {
                var c = pair.Key;
                var sb = new StringBuilder();
                foreach (var u in pair.Value)
                {
                    sb.Append(u);
                    if (pair.Value.Count != pair.Value.IndexOf(u) + 1) sb.Append(',');
                }
                var cl = new XAttribute("class", c);
                var xel = new XElement("Abnormals", cl, sb.ToString());
                result.Add(xel);
            }
            return result;
        }
        private static XElement BuildChatWindowSettings(string v)
        {
            var result = new XElement("ChatWindows");
            ChatWindowManager.Instance.ChatWindows.ToList().ForEach(cw =>
            {
                if (cw.VM.Tabs.Count == 0) return;
                cw.UpdateSettings();
                result.Add(new XElement("ChatWindow", cw.WindowSettings.ToXElement("Settings")));
            });
            return result;
        }

    }
}
