﻿using System;
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
			RealYggdrasil yggdrasil = new RealYggdrasil();
			HostsFile hosts = new HostsFile();
			YggdrasilServer server = new YggdrasilServer(56195, yggdrasil);
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

