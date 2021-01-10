﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.PowerToys.Settings.UI.Lib.Helpers;
using Microsoft.PowerToys.Settings.UI.Lib.ViewModels.Commands;

namespace Microsoft.PowerToys.Settings.UI.Lib.ViewModels
{
    public class FancyZonesViewModel : Observable
    {
        private const string ModuleName = "FancyZones";

        public ButtonClickCommand LaunchEditorEventHandler { get; set; }

        private FancyZonesSettings Settings { get; set; }

        private Func<string, int> SendConfigMSG { get; }

        private string settingsConfigFileFolder = string.Empty;

        public FancyZonesViewModel(Func<string, int> ipcMSGCallBackFunc, string configFileSubfolder = "")
        {
            settingsConfigFileFolder = configFileSubfolder;

            try
            {
                Settings = SettingsUtils.GetSettings<FancyZonesSettings>(GetSettingsSubPath());
            }
            catch
            {
                Settings = new FancyZonesSettings();
                SettingsUtils.SaveSettings(Settings.ToJsonString(), GetSettingsSubPath());
            }

            LaunchEditorEventHandler = new ButtonClickCommand(LaunchEditor);

            _shiftDrag = Settings.Properties.FancyzonesShiftDrag.Value;
            _mouseSwitch = Settings.Properties.FancyzonesMouseSwitch.Value;
            _overrideSnapHotkeys = Settings.Properties.FancyzonesOverrideSnapHotkeys.Value;
            _moveWindowsAcrossMonitors = Settings.Properties.FancyzonesMoveWindowsAcrossMonitors.Value;
            _moveWindowsBasedOnPosition = Settings.Properties.FancyzonesMoveWindowsBasedOnPosition.Value;
            _displayChangemoveWindows = Settings.Properties.FancyzonesDisplayChangeMoveWindows.Value;
            _zoneSetChangeMoveWindows = Settings.Properties.FancyzonesZoneSetChangeMoveWindows.Value;
            _appLastZoneMoveWindows = Settings.Properties.FancyzonesAppLastZoneMoveWindows.Value;
            _openWindowOnActiveMonitor = Settings.Properties.FancyzonesOpenWindowOnActiveMonitor.Value;
            _restoreSize = Settings.Properties.FancyzonesRestoreSize.Value;
            _useCursorPosEditorStartupScreen = Settings.Properties.UseCursorposEditorStartupscreen.Value;
            _showOnAllMonitors = Settings.Properties.FancyzonesShowOnAllMonitors.Value;
            _makeDraggedWindowTransparent = Settings.Properties.FancyzonesMakeDraggedWindowTransparent.Value;
            _highlightOpacity = Settings.Properties.FancyzonesHighlightOpacity.Value;
            _excludedApps = Settings.Properties.FancyzonesExcludedApps.Value;
            EditorHotkey = Settings.Properties.FancyzonesEditorHotkey.Value;

            // set the callback functions value to hangle outgoing IPC message.
            SendConfigMSG = ipcMSGCallBackFunc;

            string inactiveColor = Settings.Properties.FancyzonesInActiveColor.Value;
            _zoneInActiveColor = inactiveColor != string.Empty ? inactiveColor : "#F5FCFF";

            string borderColor = Settings.Properties.FancyzonesBorderColor.Value;
            _zoneBorderColor = borderColor != string.Empty ? borderColor : "#FFFFFF";

            string highlightColor = Settings.Properties.FancyzonesZoneHighlightColor.Value;
            _zoneHighlightColor = highlightColor != string.Empty ? highlightColor : "#0078D7";

            GeneralSettings generalSettings;
            try
            {
                generalSettings = SettingsUtils.GetSettings<GeneralSettings>(string.Empty);
            }
            catch
            {
                generalSettings = new GeneralSettings();
                SettingsUtils.SaveSettings(generalSettings.ToJsonString(), string.Empty);
            }

            _isEnabled = generalSettings.Enabled.FancyZones;
        }

        private bool _isEnabled;
        private bool _shiftDrag;
        private bool _mouseSwitch;
        private bool _overrideSnapHotkeys;
        private bool _moveWindowsAcrossMonitors;
        private bool _moveWindowsBasedOnPosition;
        private bool _displayChangemoveWindows;
        private bool _zoneSetChangeMoveWindows;
        private bool _appLastZoneMoveWindows;
        private bool _openWindowOnActiveMonitor;
        private bool _spanZonesAcrossMonitors;
        private bool _restoreSize;
        private bool _useCursorPosEditorStartupScreen;
        private bool _showOnAllMonitors;
        private bool _makeDraggedWindowTransparent;

        private int _highlightOpacity;
        private string _excludedApps;
        private HotkeySettings _editorHotkey;
        private string _zoneInActiveColor;
        private string _zoneBorderColor;
        private string _zoneHighlightColor;

        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }

            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    GeneralSettings generalSettings = SettingsUtils.GetSettings<GeneralSettings>(string.Empty);
                    generalSettings.Enabled.FancyZones = value;
                    OutGoingGeneralSettings snd = new OutGoingGeneralSettings(generalSettings);

                    SendConfigMSG(snd.ToString());
                    OnPropertyChanged("IsEnabled");
                    OnPropertyChanged("SnapHotkeysCategoryEnabled");
                }
            }
        }

        public bool SnapHotkeysCategoryEnabled
        {
            get
            {
                return _isEnabled && _overrideSnapHotkeys;
            }
        }

        public bool ShiftDrag
        {
            get
            {
                return _shiftDrag;
            }

            set
            {
                if (value != _shiftDrag)
                {
                    _shiftDrag = value;
                    Settings.Properties.FancyzonesShiftDrag.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool MouseSwitch
        {
            get
            {
                return _mouseSwitch;
            }

            set
            {
                if (value != _mouseSwitch)
                {
                    _mouseSwitch = value;
                    Settings.Properties.FancyzonesMouseSwitch.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string GetSettingsSubPath()
        {
            return settingsConfigFileFolder + "\\" + ModuleName;
        }

        public bool OverrideSnapHotkeys
        {
            get
            {
                return _overrideSnapHotkeys;
            }

            set
            {
                if (value != _overrideSnapHotkeys)
                {
                    _overrideSnapHotkeys = value;
                    Settings.Properties.FancyzonesOverrideSnapHotkeys.Value = value;
                    RaisePropertyChanged();
                    OnPropertyChanged("SnapHotkeysCategoryEnabled");
                }
            }
        }

        public bool MoveWindowsAcrossMonitors
        {
            get
            {
                return _moveWindowsAcrossMonitors;
            }

            set
            {
                if (value != _moveWindowsAcrossMonitors)
                {
                    _moveWindowsAcrossMonitors = value;
                    Settings.Properties.FancyzonesMoveWindowsAcrossMonitors.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool MoveWindowsBasedOnPosition
        {
            get
            {
                return _moveWindowsBasedOnPosition;
            }

            set
            {
                if (value != _moveWindowsBasedOnPosition)
                {
                    _moveWindowsBasedOnPosition = value;
                    Settings.Properties.FancyzonesMoveWindowsBasedOnPosition.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool DisplayChangeMoveWindows
        {
            get
            {
                return _displayChangemoveWindows;
            }

            set
            {
                if (value != _displayChangemoveWindows)
                {
                    _displayChangemoveWindows = value;
                    Settings.Properties.FancyzonesDisplayChangeMoveWindows.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ZoneSetChangeMoveWindows
        {
            get
            {
                return _zoneSetChangeMoveWindows;
            }

            set
            {
                if (value != _zoneSetChangeMoveWindows)
                {
                    _zoneSetChangeMoveWindows = value;
                    Settings.Properties.FancyzonesZoneSetChangeMoveWindows.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool AppLastZoneMoveWindows
        {
            get
            {
                return _appLastZoneMoveWindows;
            }

            set
            {
                if (value != _appLastZoneMoveWindows)
                {
                    _appLastZoneMoveWindows = value;
                    Settings.Properties.FancyzonesAppLastZoneMoveWindows.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool OpenWindowOnActiveMonitor
        {
            get
            {
                return _openWindowOnActiveMonitor;
            }

            set
            {
                if (value != _openWindowOnActiveMonitor)
                {
                    _openWindowOnActiveMonitor = value;
                    Settings.Properties.FancyzonesOpenWindowOnActiveMonitor.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool RestoreSize
        {
            get
            {
                return _restoreSize;
            }

            set
            {
                if (value != _restoreSize)
                {
                    _restoreSize = value;
                    Settings.Properties.FancyzonesRestoreSize.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool UseCursorPosEditorStartupScreen
        {
            get
            {
                return _useCursorPosEditorStartupScreen;
            }

            set
            {
                if (value != _useCursorPosEditorStartupScreen)
                {
                    _useCursorPosEditorStartupScreen = value;
                    Settings.Properties.UseCursorposEditorStartupscreen.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ShowOnAllMonitors
        {
            get
            {
                return _showOnAllMonitors;
            }

            set
            {
                if (value != _showOnAllMonitors)
                {
                    _showOnAllMonitors = value;
                    Settings.Properties.FancyzonesShowOnAllMonitors.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool SpanZonesAcrossMonitors
        {
            get
            {
                return _spanZonesAcrossMonitors;
            }

            set
            {
                if (value != _spanZonesAcrossMonitors)
                {
                    _spanZonesAcrossMonitors = value;
                    Settings.Properties.FancyzonesSpanZonesAcrossMonitors.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool MakeDraggedWindowsTransparent
        {
            get
            {
                return _makeDraggedWindowTransparent;
            }

            set
            {
                if (value != _makeDraggedWindowTransparent)
                {
                    _makeDraggedWindowTransparent = value;
                    Settings.Properties.FancyzonesMakeDraggedWindowTransparent.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ZoneHighlightColor
        {
            get
            {
                return _zoneHighlightColor;
            }

            set
            {
                if (!value.Equals(_zoneHighlightColor))
                {
                    _zoneHighlightColor = value;
                    Settings.Properties.FancyzonesZoneHighlightColor.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ZoneBorderColor
        {
            get
            {
                return _zoneBorderColor;
            }

            set
            {
                if (!value.Equals(_zoneBorderColor, StringComparison.OrdinalIgnoreCase))
                {
                    _zoneBorderColor = value;
                    Settings.Properties.FancyzonesBorderColor.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ZoneInActiveColor
        {
            get
            {
                return _zoneInActiveColor;
            }

            set
            {
                if (!value.Equals(_zoneInActiveColor))
                {
                    _zoneInActiveColor = value;
                    Settings.Properties.FancyzonesInActiveColor.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int HighlightOpacity
        {
            get
            {
                return _highlightOpacity;
            }

            set
            {
                if (value != _highlightOpacity)
                {
                    _highlightOpacity = value;
                    Settings.Properties.FancyzonesHighlightOpacity.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        public HotkeySettings EditorHotkey
        {
            get
            {
                return _editorHotkey;
            }

            set
            {
                if (value != _editorHotkey)
                {
                    if (value.IsEmpty())
                    {
                        _editorHotkey = FZConfigProperties.DefaultHotkeyValue;
                    }
                    else
                    {
                        _editorHotkey = value;
                    }

                    Settings.Properties.FancyzonesEditorHotkey.Value = _editorHotkey;
                    RaisePropertyChanged();
                }
            }
        }

        public string ExcludedApps
        {
            get
            {
                return _excludedApps;
            }

            set
            {
                if (value != _excludedApps)
                {
                    _excludedApps = value;
                    Settings.Properties.FancyzonesExcludedApps.Value = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void LaunchEditor()
        {
            // send message to launch the zones editor;
            SendConfigMSG("{\"action\":{\"FancyZones\":{\"action_name\":\"ToggledFZEditor\", \"value\":\"\"}}}");
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
            if (SendConfigMSG != null)
            {
                SndFancyZonesSettings outsettings = new SndFancyZonesSettings(Settings);
                SndModuleSettings<SndFancyZonesSettings> ipcMessage = new SndModuleSettings<SndFancyZonesSettings>(outsettings);
                SendConfigMSG(ipcMessage.ToJsonString());
            }
        }
    }
}
