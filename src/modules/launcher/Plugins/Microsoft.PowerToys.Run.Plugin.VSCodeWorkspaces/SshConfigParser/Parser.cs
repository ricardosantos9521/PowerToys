﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.PowerToys.Run.Plugin.VSCodeWorkspaces.SshConfigParser
{
    public class SshConfig
    {
        private static readonly Regex SSH_CONFIG = new Regex(@"^(\w[\s\S]*?\w)$(?=(?:\s+^\w|\z))", RegexOptions.Multiline);
        private static readonly Regex KEY_VALUE = new Regex(@"(\w+\s\S+)", RegexOptions.Multiline);

        public static IEnumerable<SshHost> ParseFile(string path)
        {
            return Parse(File.ReadAllText(path));
        }

        public static IEnumerable<SshHost> Parse(string str)
        {

            str = str.Replace("\r", "");
            var list = new List<SshHost>();
            foreach (Match match in SSH_CONFIG.Matches(str))
            {
                var sshHost = new SshHost();
                string content = match.Groups.Values.ToList()[0].Value;
                foreach (Match match1 in KEY_VALUE.Matches(content))
                {
                    var split = match1.Value.Split(" ");
                    var key = split[0];
                    var value = split[1];
                    sshHost.Properties[key] = value;
                }
                list.Add(sshHost);
            }
            return list;
        }
    }
}