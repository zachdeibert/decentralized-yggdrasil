using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class ServerCommand : ICommand {
		public string Name {
			get {
				return "server";
			}
		}

		public void Run(string[] args) {
			RealYggdrasil yggdrasil = new RealYggdrasil();
			HostsFile hosts = new HostsFile();
			YggdrasilServer server = new YggdrasilServer(56195, yggdrasil);
			server.Start();
			Process ssl;
			UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
			string exe = Path.Combine(Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path)), "ssl-endpoint.exe");
			if (Type.GetType("Mono.Runtime") == null) {
				ssl = Process.Start(exe, "*.mojang.com 443 localhost 56195");
			} else {
				ssl = Process.Start("mono", string.Concat("\"", exe, "\" *.mojang.com 443 localhost 56195"));
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
			AppDomain.CurrentDomain.ProcessExit += (sender, e) => server.Stop();
			try {
				while (true) {
					Thread.Sleep(int.MaxValue);
				}
			} catch {
			}
		}
	}
}

