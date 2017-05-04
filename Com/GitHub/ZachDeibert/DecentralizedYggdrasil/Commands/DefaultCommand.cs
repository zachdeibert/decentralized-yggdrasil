using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class DefaultCommand : ICommand {
		public string Name {
			get {
				return null;
			}
		}

		public void Run(string[] args) {
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

