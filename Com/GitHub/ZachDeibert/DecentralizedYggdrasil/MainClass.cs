using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	class MainClass {
		public static void Main(string[] args) {
			if (args.Length == 1 && args[0] == "server") {
				YggdrasilServer server = new YggdrasilServer(56195);
				server.Start();
				Process ssl;
				if (Type.GetType("Mono.Runtime") == null) {
					ssl = Process.Start("ssl-endpoint.exe", "*.mojang.com 443 localhost 56195");
				} else {
					ssl = Process.Start("mono", "ssl-endpoint.exe *.mojang.com 443 localhost 56195");
				}
				Thread thread = Thread.CurrentThread;
				server.OnStop += () => {
					ssl.Kill();
					thread.Interrupt();
				};
				try {
					while (true) {
						Thread.Sleep(int.MaxValue);
					}
				} catch {
				}
			} else {
				try {
					WebRequest.Create("http://localhost:56195/cghzddy/lock").GetResponse().Close();
				} catch {
					ProcessStartInfo info;
					switch (Environment.OSVersion.Platform) {
						case PlatformID.MacOSX:
						case PlatformID.Unix:
							info = new ProcessStartInfo("sudo", string.Concat("mono ", Assembly.GetExecutingAssembly().CodeBase, " server"));
							break;
						default:
							if (Type.GetType("Mono.Runtime") == null) {
								info = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase, "server");
							} else {
								info = new ProcessStartInfo("mono", string.Concat(Assembly.GetExecutingAssembly().CodeBase, " server"));
							}
							info.Verb = "RunAs";
							break;
					}
					Process.Start(info);
					Thread.Sleep(5000);
					WebRequest.Create("http://localhost:56195/cghzddy/lock").GetResponse().Close();
				}
				X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
				store.Open(OpenFlags.ReadOnly);
				X509Certificate2 cert = store.Certificates.Cast<X509Certificate2>().First(c => c.Subject == "CN=*.mojang.com");
				File.WriteAllBytes("mojang.cer", cert.Export(X509ContentType.Cert));
				Process mc = Process.Start("java", "-jar decentralized-yggdrasil.jar mojang.cer");
				mc.WaitForExit();
				WebRequest.Create("http://localhost:56195/cghzddy/unlock").GetResponse().Close();
			}
		}
	}
}
