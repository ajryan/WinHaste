using System.Runtime.InteropServices;

namespace WinHaste
{
  // https://stackoverflow.com/questions/44205260/net-core-copy-to-clipboard
  public static class Clipboard
  {
    public static bool Copy(string text)
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        $"echo {text} | clip".Bat();
        return true;
      }

      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      {
        $"echo \"{text}\" | pbcopy".Bash();
        return true;
      }

      return false;
    }
  }
}