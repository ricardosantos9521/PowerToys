// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Wox.Plugin.SharedCommands
{
    public static class SearchWeb
    {
        /// <summary>
        /// Opens search in a new browser. If no browser path is passed in then Chrome is used.
        /// Leave browser path blank to use Chrome.
        /// </summary>
        public static void NewBrowserWindow(this Uri url, string browserPath)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var browserExecutableName = browserPath?
                                        .Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.None)
                                        .Last();

            var browser = string.IsNullOrEmpty(browserExecutableName) ? "chrome" : browserPath;

            // Internet Explorer will open url in new browser window, and does not take the --new-window parameter
            var browserArguments = browserExecutableName == "iexplore.exe" ? url.AbsoluteUri : "--new-window " + url.AbsoluteUri;

            try
            {
                Process.Start(browser, browserArguments);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url.AbsoluteUri,
                    UseShellExecute = true,
                };
                Process.Start(psi);
            }
        }

        /// <summary>
        /// Opens search as a tab in the default browser chosen in Windows settings.
        /// </summary>
        public static void NewTabInBrowser(this Uri url, string browserPath)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            try
            {
                if (!string.IsNullOrEmpty(browserPath))
                {
                    Process.Start(browserPath, url.AbsoluteUri);
                }
                else
                {
                    Process.Start(url.AbsoluteUri);
                }
            }

            // This error may be thrown for Process.Start(browserPath, url)
            catch (System.ComponentModel.Win32Exception)
            {
                Process.Start(url.AbsoluteUri);
            }
        }
    }
}
