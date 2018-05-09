using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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
				ssl = Process.Start(exe, "*.mojang.com 443 127.0.0.1 56195");
			} else {
				ssl = Process.Start("mono", string.Concat("\"", exe, "\" *.mojang.com 443 127.0.0.1 56195"));
			}
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			X509Certificate2 cert;
			do {
				store.Open(OpenFlags.ReadOnly);
				cert = store.Certificates.Cast<X509Certificate2>().FirstOrDefault(c => c.Subject == "CN=*.mojang.com");
			} while (cert == null);
			File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(exe), "mojang.cer"), cert.Export(X509ContentType.Cert));
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

