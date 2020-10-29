﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Microsoft.Plugin.Folder.Sources
{
    public class FolderHelper : IFolderHelper
    {
        private readonly IDriveInformation _driveInformation;
        private readonly IFolderLinks _folderLinks;

        public FolderHelper(IDriveInformation driveInformation, IFolderLinks folderLinks)
        {
            _driveInformation = driveInformation;
            _folderLinks = folderLinks;
        }

        public IEnumerable<FolderLink> GetUserFolderResults(string query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(paramName: nameof(query));
            }

            return _folderLinks.FolderLinks()
                .Where(x => x.Nickname.StartsWith(query, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsDriveOrSharedFolder(string search)
        {
            if (search == null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            if (search.StartsWith(@"\\", StringComparison.InvariantCulture))
            { // share folder
                return true;
            }

            var driverNames = _driveInformation.GetDriveNames()
                .ToImmutableArray();

            if (driverNames.Any())
            {
                if (driverNames.Any(dn => search.StartsWith(dn, StringComparison.InvariantCultureIgnoreCase)))
                {
                    // normal drive letter
                    return true;
                }
            }
            else
            {
                if (search.Length > 2 && ValidDriveLetter(search[0]) && search[1] == ':')
                { // when we don't have the drive letters we can try...
                    return true; // we don't know so let's give it the possibility
                }
            }

            return false;
        }

        /// <summary>
        /// This check is needed because char.IsLetter accepts more than [A-z]
        /// </summary>
        public static bool ValidDriveLetter(char c)
        {
            return c <= 122 && char.IsLetter(c);
        }

        public static string Expand(string search)
        {
            return Environment.ExpandEnvironmentVariables(search);
        }
    }
}
