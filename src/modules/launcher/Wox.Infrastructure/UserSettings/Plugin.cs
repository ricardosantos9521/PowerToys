﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Wox.Infrastructure.UserSettings
{
    public class Plugin
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public List<string> ActionKeywords { get; set; } // a reference of the action keywords from plugin manager

        /// <summary>
        /// Gets or sets a value indicating whether used only to save the state of the plugin in settings
        /// </summary>
        public bool Disabled { get; set; }
    }
}
