﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TCC.Data;

namespace TCC.Converters
{
    public class ClassToFillConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var c = (Class?)value ?? Class.Common;
            switch (c)
            {
                case Class.Lancer:
                    return Application.Current.FindResource("TankRoleColor");
                case Class.Brawler:
                    return Application.Current.FindResource("TankRoleColor");
                case Class.Priest:
                    return Application.Current.FindResource("HealerRoleColor");
                case Class.Mystic:
                    return Application.Current.FindResource("HealerRoleColor");
                default:
                    return Application.Current.FindResource("DpsRoleColor");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}