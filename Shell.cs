using System.Diagnostics;

namespace WinHaste
{
  // https://stackoverflow.com/questions/44205260/net-core-copy-to-clipboard
  public static class Shell
  {
    public static string Bash(this string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");
      string result = Run("/bin/bash", $"-c \"{escapedArgs}\"");
      return result;
    }

    public static string Bat(this string cmd)
    {
      var escapedArgs = cmd.Replace("\"", "\\\"");
      string result = Run("cmd.exe", $"/c \"{escapedArgs}\"");
      return result;
    }

    private static string Run(string filename, string arguments)
    {
      var process = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = filename,
          Arguments = arguments,
          RedirectStandardOutput = true,
          UseShellExecute = false,
          CreateNoWindow = false,
        }
      };

      process.Start();
      string result = process.StandardOutput.ReadToEnd();
      process.WaitForExit();

      return result;
    }
  }
}