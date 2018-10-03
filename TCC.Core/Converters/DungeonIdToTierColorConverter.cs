﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TCC.Data;

namespace TCC.Converters
{
    internal class DungeonIdToTierColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var id = (uint?)value ?? 0;
            if(!SessionManager.DungeonDatabase.DungeonDefs.ContainsKey(id)) return Application.Current.FindResource("TierSoloDungeonColor");
            var t = SessionManager.DungeonDatabase.DungeonDefs[id].Tier;
            switch (t)
            {
                case DungeonTier.Tier2:
                    return Application.Current.FindResource("Tier2DungeonColor");
                case DungeonTier.Tier3:
                    return Application.Current.FindResource("Tier3DungeonColor");
                case DungeonTier.Tier4:
                    return Application.Current.FindResource("Tier4DungeonColor");
                case DungeonTier.Tier5:
                    return Application.Current.FindResource("Tier5DungeonColor");
                default:
                    return Application.Current.FindResource("TierSoloDungeonColor");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
