using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinHaste
{
  class Program
  {
    private static readonly Regex _HasteKeyRegex = new Regex(@"{""key"":""(?<key>[a-z].*)""}", RegexOptions.Compiled);

    [STAThread]
    static async Task Main(string[] args)
    {
      var parameters = new Parameters(args);
      var parseResult = parameters.Parse();
      if (parseResult != Parameters.ParseResult.Success)
      {
        Console.WriteLine(parameters.Usage);
        Console.WriteLine(Environment.NewLine + "ERROR: " + parseResult);
        return;
      }

      var haste = !String.IsNullOrEmpty(parameters.FilePath)
        ? ReadFile(parameters.FilePath)
        : ReadStdin();

      Console.WriteLine($"{Environment.NewLine}Uploading...");

      using (var client = new WebClient())
      {
        var response = await client.UploadStringTaskAsync(parameters.Url + "/documents", haste);
        var match = _HasteKeyRegex.Match(response);

        if (!match.Success)
        {
          Console.WriteLine(response);
          return;
        }

        string hasteUrl = String.Concat(parameters.Url, "/", match.Groups["key"]);

        string copyMessage = Clipboard.Copy(hasteUrl) ? " (copied to clipboard)" : string.Empty;

        Console.WriteLine($"Haste URL: {hasteUrl}{copyMessage}{Environment.NewLine}", hasteUrl);
      }
    }

    private static string ReadFile(string filePath)
    {
      using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
      {
        return streamReader.ReadToEnd();
      }
    }

    private static string ReadStdin()
    {
      bool piped = IsInputPiped();

      var haste = String.Empty;

      int consoleKey = Console.Read();
      while (consoleKey != -1)
      {
        var consoleChar = Convert.ToChar(consoleKey);

        if (piped)
        {
          Console.Write(consoleChar);
          Console.Out.Flush();
        }

        haste += consoleChar;

        consoleKey = Console.Read();
      }

      return haste;
    }

    private static bool IsInputPiped()
    {
      try
      {
        var tmp = Console.KeyAvailable;
        return false;
      }
      catch (InvalidOperationException)
      {
        return true;
      }
    }
  }
}
