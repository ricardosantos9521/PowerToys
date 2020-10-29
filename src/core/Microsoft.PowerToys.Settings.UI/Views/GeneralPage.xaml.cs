// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.PowerToys.Settings.UI.Library;
using Microsoft.PowerToys.Settings.UI.Library.Utilities;
using Microsoft.PowerToys.Settings.UI.Library.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.Data.Json;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.PowerToys.Settings.UI.Views
{
    /// <summary>
    /// General Settings Page.
    /// </summary>
    public sealed partial class GeneralPage : Page
    {
        /// <summary>
        /// Gets or sets view model.
        /// </summary>
        public GeneralViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralPage"/> class.
        /// General Settings page constructor.
        /// </summary>
        public GeneralPage()
        {
            InitializeComponent();

            // Load string resources
            ResourceLoader loader = ResourceLoader.GetForViewIndependentUse();
            var settingsUtils = new SettingsUtils(new SystemIOProvider());

            ViewModel = new GeneralViewModel(
                SettingsRepository<GeneralSettings>.GetInstance(settingsUtils),
                loader.GetString("GeneralSettings_RunningAsAdminText"),
                loader.GetString("GeneralSettings_RunningAsUserText"),
                ShellPage.IsElevated,
                ShellPage.IsUserAnAdmin,
                UpdateUIThemeMethod,
                ShellPage.SendDefaultIPCMessage,
                ShellPage.SendRestartAdminIPCMessage,
                ShellPage.SendCheckForUpdatesIPCMessage);

            ShellPage.ShellHandler.IPCResponseHandleList.Add((JsonObject json) =>
            {
                try
                {
                    string version = json.GetNamedString("version");
                    bool isLatest = json.GetNamedBoolean("isVersionLatest");

                    var str = string.Empty;
                    if (isLatest)
                    {
                        str = ResourceLoader.GetForCurrentView().GetString("GeneralSettings_VersionIsLatest");
                    }
                    else if (version != string.Empty)
                    {
                        str = ResourceLoader.GetForCurrentView().GetString("GeneralSettings_NewVersionIsAvailable");
                        if (str != string.Empty)
                        {
                            str += ": " + version;
                        }
                    }

                    ViewModel.LatestAvailableVersion = string.Format(str);
                }
                catch (Exception)
                {
                }
            });

            DataContext = ViewModel;
        }

        public int UpdateUIThemeMethod(string themeName)
        {
            switch (themeName.ToUpperInvariant())
            {
                case "LIGHT":
                    ShellPage.ShellHandler.RequestedTheme = ElementTheme.Light;
                    break;
                case "DARK":
                    ShellPage.ShellHandler.RequestedTheme = ElementTheme.Dark;
                    break;
                case "SYSTEM":
                    ShellPage.ShellHandler.RequestedTheme = ElementTheme.Default;
                    break;
            }

            return 0;
        }
    }
}
