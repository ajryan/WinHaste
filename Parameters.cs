using System;
using System.IO;

namespace WinHaste
{
  internal class Parameters
  {
    public enum ParseResult
    {
      Success,
      BadUrl,
      BadFilename,
      BadArgs
    }

    private const string DEFAULT_URL = "https://hastebin.com";

    private const string USAGE =
@"Usage: WinHaste.exe [service url] [file to haste]

Defaults:
                    [service url] = https://hastebin.com

                    When [file to haste] is omitted, haste.exe reads from
                    standard input until an end-of-file character is read.";

    private readonly string[] _args;

    public Parameters(string[] args)
    {
      _args = args;
    }

    public string Usage => USAGE;
    public string Url { get; private set; }
    public string FilePath { get; private set; }

    public ParseResult Parse()
    {
      if (_args.Length == 0)
      {
        return ParseResult.Success;
      }

      if (_args.Length > 2)
        return ParseResult.BadArgs;

      bool pathProvided = false;

      if (_args.Length > 1)
      {
        Url = ParseUrl(_args[0]);
        FilePath = _args[1];
        pathProvided = true;
      }
      else
      {
        Url = ParseUrl(_args[0]);

        if (Url == null)
        {
          Url = DEFAULT_URL;
          FilePath = _args[0];
          pathProvided = true;
        }
      }

      return Url == null ? ParseResult.BadUrl
        : !File.Exists(FilePath) && pathProvided
          ? ParseResult.BadFilename
          : ParseResult.Success;
    }

    private string ParseUrl(string candidate)
    {
      if (!Uri.TryCreate(candidate, UriKind.Absolute, out var tmpUri))
        return null;

      if (tmpUri.Scheme != "http" && tmpUri.Scheme != "https")
        return null;

      return candidate.TrimEnd('/');
    }
  }
}
