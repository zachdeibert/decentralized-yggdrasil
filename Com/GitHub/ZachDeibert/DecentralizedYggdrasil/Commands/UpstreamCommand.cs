using System;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class UpstreamCommand : ICommand {
		public string Name {
			get {
				return "upstream";
			}
		}

		private string AppendDefaultPortAndProto(string host) {
			if (host.Contains("://")) {
				if (host.Count(c => c == ':') == 1) {
					return string.Concat(host, ":56195");
				} else {
					return host;
				}
			} else {
				switch (host.Count(c => c == ':')) {
				case 0:
					return string.Concat("http://", host, ":56195");
				case 1:
					return string.Concat("http://", host);
				default:
					if (host.Contains("[")) {
						if (host.Contains("]:")) {
							return string.Concat("http://", host);
						} else {
							return string.Concat("http://", host, ":56195");
						}
					} else {
						return string.Concat("http://[", host, "]:56195");
					}
				}
			}
		}

		public void Run(string[] args) {
			switch (args.Length) {
			case 0: {
					TransientStateData data = TransientStateData.Load();
					foreach (string server in data.UpstreamServers) {
						Console.WriteLine(server);
					}
					TransientStateData.Save(data);
				} break;
			case 1: {
					TransientStateData data = TransientStateData.Load();
					if (args[0].StartsWith("-")) {
						data.UpstreamServers.Remove(AppendDefaultPortAndProto(args[0].Substring(1)));
					} else {
						data.UpstreamServers.Add(AppendDefaultPortAndProto(args[0]));
					}
					TransientStateData.Save(data);
				} break;
			default:
				Console.Error.WriteLine("Usage: decentralized-yggdrasil.exe upstream (-)[server ip]");
				break;
			}
		}
	}
}

