using System;
using System.Diagnostics;
using System.Threading;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class ServerCommand : ICommand {
		public string Name {
			get {
				return "server";
			}
		}

		public void Run(string[] args) {
			HostsFile hosts = new HostsFile();
			YggdrasilServer server = new YggdrasilServer(56195);
			server.Start();
			Process ssl;
			if (Type.GetType("Mono.Runtime") == null) {
				ssl = Process.Start("ssl-endpoint.exe", "*.mojang.com 443 localhost 56195");
			} else {
				ssl = Process.Start("mono", "ssl-endpoint.exe *.mojang.com 443 localhost 56195");
			}
			Thread thread = Thread.CurrentThread;
			if (!hosts.IsOverriden) {
				hosts.Override();
			}
			server.OnStop += () => {
				ssl.Kill();
				thread.Interrupt();
				hosts.Reset();
			};
			try {
				while (true) {
					Thread.Sleep(int.MaxValue);
				}
			} catch {
			}
		}
	}
}

