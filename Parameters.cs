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

		private const string DEFAULT_URL = "http://hastebin.com";

		private const string USAGE =
@"Usage: WinHaste.exe [service url] [file to haste]
				
Defaults:
			[service url] = http://hastebin.com
				
			When [file to haste] is omitted, haste.exe reads from
			standard input until an end-of-file character (^Z) is
			encountered.";

		private readonly string[] _args;

		public Parameters(string[] args)
		{
			_args = args;
		}

		public string Usage { get { return USAGE; } }
		public string Url { get; private set; }
		public string Filename { get; private set; }

		public ParseResult Parse()
		{
			if (_args.Length == 0)
			{
				Url = DEFAULT_URL;
				return ParseResult.Success;
			}

			if (_args.Length > 2)
				return ParseResult.BadArgs;

			if (_args.Length > 0)
			{
				Uri tmpUri;
				if (!Uri.TryCreate(_args[0], UriKind.Absolute, out tmpUri))
					return ParseResult.BadUrl;

				if (tmpUri.Scheme != "http" && tmpUri.Scheme != "https")
					return ParseResult.BadUrl;

				Url = _args[0].TrimEnd('/');
			}

			if (_args.Length > 1)
			{
				if (!File.Exists(_args[1]))
					return ParseResult.BadFilename;

				Filename = _args[1];
			}

			return ParseResult.Success;
		}
	}
}
