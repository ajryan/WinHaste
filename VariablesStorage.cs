using System;
using System.Collections.Generic;
using System.IO;

namespace WinHaste
{
    class VariablesStorage
    {
        private static string _directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.winhaste";
        private static string _configPath = _directoryPath + "/config.ini";
        private static string _defaultConfig = "baseUrl=https://hastebin.com";

        public static bool IsDirectoryExists()
        {
            if (!Directory.Exists(_directoryPath))
            {
                Directory.CreateDirectory(_directoryPath);
            }
            return File.Exists(_configPath);
        }

        public static void InitConfigFile()
        {
            if (IsDirectoryExists()) return;
            using (StreamWriter fs = File.CreateText(_configPath))
            {
                fs.Write(_defaultConfig);
            }
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