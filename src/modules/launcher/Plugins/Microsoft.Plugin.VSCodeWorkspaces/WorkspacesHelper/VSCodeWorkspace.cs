﻿using Microsoft.Plugin.VSCodeWorkspaces.VSCodeHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Plugin.VSCodeWorkspaces.WorkspacesHelper
{
    public class VSCodeWorkspace
    {
        public string Path { get; set; }

        public string RelativePath { get; set; }

        public string FolderName { get; set; }

        public string ExtraInfo { get; set; }

        public TypeWorkspace TypeWorkspace { get; set; }

        public VSCodeInstance VSCodeInstance { get; set; }

        public string WorkspaceTypeToString()
        {
            switch (TypeWorkspace)
            {
                case TypeWorkspace.Local: return "Local";
                case TypeWorkspace.Codespaces: return "Codespaces";
                case TypeWorkspace.RemoteContainers: return "Container";
                case TypeWorkspace.RemoteSSH: return "SSH";
                case TypeWorkspace.RemoteWSL: return "WSL";
            }

            return string.Empty;
        }
    }

    public enum TypeWorkspace
    {
        Local = 1,
        Codespaces = 2,
        RemoteWSL = 3,
        RemoteSSH = 4,
        RemoteContainers = 5
    }
}
