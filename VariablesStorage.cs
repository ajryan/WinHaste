using System;
using System.Collections.Generic;
using System.IO;

namespace WinHaste
{
    class VariablesStorage
    {
        private static string _configPath = "%UserName%/.winhaste/config.json";
        private static string _defaultConfig = "baseUrl=https://hastebin.com";

        public static bool IsDirectoryExists()
        {
            return File.Exists(_configPath);
        }

        public static void InitConfigFile()
        {
            if (IsDirectoryExists()) return;
            File.WriteAllText(_configPath, _defaultConfig);
        }

        public static Dictionary<String, String> GetConfig()
        {
            Dictionary<String, String> config = new Dictionary<string, string>();
            String[] configFile = File.ReadAllLines(_configPath);
            foreach (var line in configFile)
            {
                String[] splittedLine = line.Split("=");
                config[splittedLine[0]] = splittedLine[1];
            }

            return config;
        }
    }
}