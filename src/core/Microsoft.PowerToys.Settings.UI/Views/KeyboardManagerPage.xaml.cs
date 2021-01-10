﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.PowerToys.Settings.UI.Lib;
using Microsoft.PowerToys.Settings.UI.Lib.Utilities;
using Microsoft.PowerToys.Settings.UI.Lib.ViewModels;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.PowerToys.Settings.UI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KeyboardManagerPage : Page
    {
        private const string PowerToyName = "Keyboard Manager";

        private readonly CoreDispatcher dispatcher;
        private readonly FileSystemWatcher watcher;

        public KeyboardManagerViewModel ViewModel { get; }

        public KeyboardManagerPage()
        {
            dispatcher = Window.Current.Dispatcher;

            ViewModel = new KeyboardManagerViewModel(ShellPage.SendDefaultIPCMessage, FilterRemapKeysList);

            watcher = Helper.GetFileWatcher(
                PowerToyName,
                ViewModel.Settings.Properties.ActiveConfiguration.Value + ".json",
                OnConfigFileUpdate);

            InitializeComponent();
            DataContext = ViewModel;
        }

        private async void OnConfigFileUpdate()
        {
            // Note: FileSystemWatcher raise notification multiple times for single update operation.
            // Todo: Handle duplicate events either by somehow suppress them or re-read the configuration everytime since we will be updating the UI only if something is changed.
            if (ViewModel.LoadProfile())
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ViewModel.NotifyFileChanged();
                });
            }
        }

        private void CombineRemappings(List<KeysDataModel> remapKeysList, uint leftKey, uint rightKey, uint combinedKey)
        {
            KeysDataModel firstRemap = remapKeysList.Find(x => uint.Parse(x.OriginalKeys) == leftKey);
            KeysDataModel secondRemap = remapKeysList.Find(x => uint.Parse(x.OriginalKeys) == rightKey);
            if (firstRemap != null && secondRemap != null)
            {
                if (firstRemap.NewRemapKeys == secondRemap.NewRemapKeys)
                {
                    KeysDataModel combinedRemap = new KeysDataModel
                    {
                        OriginalKeys = combinedKey.ToString(),
                        NewRemapKeys = firstRemap.NewRemapKeys,
                    };
                    remapKeysList.Insert(remapKeysList.IndexOf(firstRemap), combinedRemap);
                    remapKeysList.Remove(firstRemap);
                    remapKeysList.Remove(secondRemap);
                }
            }
        }

        private int FilterRemapKeysList(List<KeysDataModel> remapKeysList)
        {
            CombineRemappings(remapKeysList, (uint)VirtualKey.LeftControl, (uint)VirtualKey.RightControl, (uint)VirtualKey.Control);
            CombineRemappings(remapKeysList, (uint)VirtualKey.LeftMenu, (uint)VirtualKey.RightMenu, (uint)VirtualKey.Menu);
            CombineRemappings(remapKeysList, (uint)VirtualKey.LeftShift, (uint)VirtualKey.RightShift, (uint)VirtualKey.Shift);
            CombineRemappings(remapKeysList, (uint)VirtualKey.LeftWindows, (uint)VirtualKey.RightWindows, Helper.VirtualKeyWindows);

            return 0;
        }
    }
}
