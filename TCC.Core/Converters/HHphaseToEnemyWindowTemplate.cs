﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TCC.Data;

namespace TCC.Converters
{
    public class HHphaseToEnemyWindowTemplate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ReSharper disable once PossibleNullReferenceException
            switch ((HarrowholdPhase)value)
            {
                case HarrowholdPhase.Phase1:
                    return Application.Current.FindResource("Phase1EnemyWindowLayout");
                case HarrowholdPhase.Phase2:
                    return Application.Current.FindResource("DefaultEnemyWindowLayout");
                case HarrowholdPhase.Phase3:
                    return Application.Current.FindResource("DefaultEnemyWindowLayout");
                case HarrowholdPhase.Phase4:
                    return Application.Current.FindResource("DefaultEnemyWindowLayout");
                case HarrowholdPhase.Balistas:
                    return Application.Current.FindResource("DefaultEnemyWindowLayout");
                default:
                    return Application.Current.FindResource("DefaultEnemyWindowLayout");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
