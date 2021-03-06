﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	public class HostsFile {
		private const string Header = "#-----START YGGDRASIL HOSTS (CGHZDDY)-----#";
		private const string Footer = "#----- END YGGDRASIL HOSTS (CGHZDDY) -----#";
		public static readonly string[] Hosts = new [] {
			"authserver.mojang.com",
			"sessionserver.mojang.com"
		};
		public readonly string FilePath;

		public bool IsOverriden {
			get {
				string[] lines = File.ReadAllLines(FilePath);
				int startLine = Array.IndexOf(lines, Header);
				int endLine = Array.IndexOf(lines, Footer);
				return startLine >= 0 && endLine > startLine;
			}
		}

		public void Override() {
			File.AppendAllLines(FilePath, Enumerable.Repeat(Header, 1).Concat(Hosts.Select(h => string.Concat("127.0.0.1\t", h))).Concat(Enumerable.Repeat(Footer, 1)));
		}

		public void Reset() {
			List<string> lines = File.ReadAllLines(FilePath).ToList();
			int startLine = lines.IndexOf(Header);
			int endLine = lines.IndexOf(Footer);
			lines.RemoveRange(startLine, endLine - startLine + 1);
			File.WriteAllLines(FilePath, lines.ToArray());
		}

		public HostsFile() {
			switch (Environment.OSVersion.Platform) {
				case PlatformID.MacOSX:
				case PlatformID.Unix:
					FilePath = "/etc/hosts";
					break;
				default:
					FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "Drivers", "etc", "hosts");
					break;
			}
		}
	}
}

