﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json;
using Microsoft.PowerToys.Settings.UI.Lib.Helpers;

namespace Microsoft.PowerToys.Settings.UI.Lib.ViewModels
{
    public class ColorPickerViewModel : Observable
    {
        private ColorPickerSettings _colorPickerSettings;
        private bool _isEnabled;

        private Func<string, int> SendConfigMSG { get; }

        public ColorPickerViewModel(Func<string, int> ipcMSGCallBackFunc)
        {
            if (SettingsUtils.SettingsExists(ColorPickerSettings.ModuleName))
            {
                _colorPickerSettings = SettingsUtils.GetSettings<ColorPickerSettings>(ColorPickerSettings.ModuleName);
            }
            else
            {
                _colorPickerSettings = new ColorPickerSettings();
            }

            if (SettingsUtils.SettingsExists())
            {
                var generalSettings = SettingsUtils.GetSettings<GeneralSettings>();
                _isEnabled = generalSettings.Enabled.ColorPicker;
            }

            // set the callback functions value to hangle outgoing IPC message.
            SendConfigMSG = ipcMSGCallBackFunc;
        }

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));

                    // grab the latest version of settings
                    var generalSettings = SettingsUtils.GetSettings<GeneralSettings>();
                    generalSettings.Enabled.ColorPicker = value;
                    OutGoingGeneralSettings outgoing = new OutGoingGeneralSettings(generalSettings);
                    SendConfigMSG(outgoing.ToString());
                }
            }
        }

        public bool ChangeCursor
        {
            get
            {
                return _colorPickerSettings.Properties.ChangeCursor;
            }

            set
            {
                if (_colorPickerSettings.Properties.ChangeCursor != value)
                {
                    _colorPickerSettings.Properties.ChangeCursor = value;
                    OnPropertyChanged(nameof(ChangeCursor));
                    NotifySettingsChanged();
                }
            }
        }

        public HotkeySettings ActivationShortcut
        {
            get
            {
                return _colorPickerSettings.Properties.ActivationShortcut;
            }

            set
            {
                if (_colorPickerSettings.Properties.ActivationShortcut != value)
                {
                    _colorPickerSettings.Properties.ActivationShortcut = value;
                    OnPropertyChanged(nameof(ActivationShortcut));
                    NotifySettingsChanged();
                }
            }
        }

        public int CopiedColorRepresentationIndex
        {
            get
            {
                return (int)_colorPickerSettings.Properties.CopiedColorRepresentation;
            }

            set
            {
                if (_colorPickerSettings.Properties.CopiedColorRepresentation != (ColorRepresentationType)value)
                {
                    _colorPickerSettings.Properties.CopiedColorRepresentation = (ColorRepresentationType)value;
                    OnPropertyChanged(nameof(CopiedColorRepresentationIndex));
                    NotifySettingsChanged();
                }
            }
        }

        private void NotifySettingsChanged()
        {
            SendConfigMSG(
                   string.Format("{{ \"powertoys\": {{ \"{0}\": {1} }} }}", ColorPickerSettings.ModuleName, JsonSerializer.Serialize(_colorPickerSettings)));
        }
    }
}
