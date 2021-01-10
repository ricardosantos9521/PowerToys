﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace Wox.Plugin
{
    [JsonObject(MemberSerialization.OptOut)]
    public class PluginMetadata : BaseModel
    {
        private static readonly IFileSystem FileSystem = new FileSystem();
        private static readonly IPath Path = FileSystem.Path;

        private string _pluginDirectory;

        private List<string> _actionKeywords;

        public PluginMetadata(List<string> actionKeywords = null)
        {
            _actionKeywords = actionKeywords;
        }

        public string ID { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public string Language { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public bool Disabled { get; set; }

        public string ExecuteFilePath { get; private set; }

        public string ExecuteFileName { get; set; }

        public string PluginDirectory
        {
            get
            {
                return _pluginDirectory;
            }

            internal set
            {
                _pluginDirectory = value;
                ExecuteFilePath = Path.Combine(value, ExecuteFileName);
                IcoPath = Path.Combine(value, IcoPath);
            }
        }

        public string ActionKeyword { get; set; }

        public List<string> GetActionKeywords()
        {
            return _actionKeywords;
        }

        public void SetActionKeywords(List<string> value)
        {
            _actionKeywords = value;
        }

        public string IcoPath { get; set; }

        public override string ToString()
        {
            return Name;
        }

        [Obsolete("Use IcoPath")]
        public string FullIcoPath => IcoPath;

        /// <summary>
        /// Gets or sets init time include both plugin load time and init time
        /// </summary>
        [JsonIgnore]
        public long InitTime { get; set; }

        [JsonIgnore]
        public long AvgQueryTime { get; set; }

        [JsonIgnore]
        public int QueryCount { get; set; }
    }
}
