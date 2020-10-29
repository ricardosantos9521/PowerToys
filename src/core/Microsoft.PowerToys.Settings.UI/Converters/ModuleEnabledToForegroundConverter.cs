﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.PowerToys.Settings.UI.Library;
using Microsoft.PowerToys.Settings.UI.Library.Utilities;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Microsoft.PowerToys.Settings.UI.Converters
{
    public sealed class ModuleEnabledToForegroundConverter : IValueConverter
    {
        private readonly ISettingsUtils settingsUtils = new SettingsUtils(new SystemIOProvider());

        private string selectedTheme = string.Empty;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isEnabled = (bool)value;

            var defaultTheme = new Windows.UI.ViewManagement.UISettings();
            var uiTheme = defaultTheme.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString();
            selectedTheme = SettingsRepository<GeneralSettings>.GetInstance(settingsUtils).SettingsConfig.Theme.ToLower();

            if (selectedTheme == "dark" || (selectedTheme == "system" && uiTheme == "#FF000000"))
            {
                // DARK
                if (isEnabled)
                {
                    return (SolidColorBrush)Application.Current.Resources["DarkForegroundBrush"];
                }
                else
                {
                    return (SolidColorBrush)Application.Current.Resources["DarkForegroundDisabledBrush"];
                }
            }
            else
            {
                // LIGHT
                if (isEnabled)
                {
                    return (SolidColorBrush)Application.Current.Resources["LightForegroundBrush"];
                }
                else
                {
                    return (SolidColorBrush)Application.Current.Resources["LightForegroundDisabledBrush"];
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
