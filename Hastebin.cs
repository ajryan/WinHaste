using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace WinHaste
{
    public static class Hastebin
    {
        public static Uri UploadSnippet(string snippet)
        {
            using (WebClient client = new WebClient())
            {
                string response = client.UploadString(Parameters.DEFAULT_URL + "/documents", snippet);

                Match match = Program._HasteKeyRegex.Match(response);

                if (!match.Success)
                    throw new InvalidOperationException("Could not upload code snippet: " + response);

                return new Uri("http://hastebin.com/" + match.Groups["key"]);
            }
        }
        public static Uri UploadFromFile(string path)
        {
            return UploadSnippet(File.ReadAllText(path));
        }
    }
}
