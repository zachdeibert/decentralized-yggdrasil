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
		private const int RetryTimeout = 250;
		private const int MaxServerStartTime = 60000;

		public string Name {
			get {
				return null;
			}
		}

		public void Run(string[] args) {
			UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
			string thisExe = Uri.UnescapeDataString(uri.Path);
			try {
				WebRequest.Create("http://localhost:56195/cghzddy/lock").GetResponse().Close();
			} catch {
				ProcessStartInfo info;
				switch (Environment.OSVersion.Platform) {
				case PlatformID.MacOSX:
				case PlatformID.Unix:
					info = new ProcessStartInfo("sudo", string.Concat("-i -A \"", thisExe, "\" server")) {
						UseShellExecute = false
					};
					info.EnvironmentVariables.Add("SUDO_ASKPASS", Path.Combine(Path.GetDirectoryName(thisExe), "askpass"));
					break;
				default:
					info = new ProcessStartInfo(thisExe, "server") {
						Verb = "RunAs"
					};
					break;
				}
				Process.Start(info);
				int slept = 0;
				while (true) {
					Thread.Sleep(RetryTimeout);
					slept += RetryTimeout;
					try {
						WebRequest.Create("http://localhost:56195/cghzddy/lock").GetResponse().Close();
						break;
					} catch (Exception ex) {
						if (slept >= MaxServerStartTime) {
							throw ex;
						}
					}
				}
			}
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);
			X509Certificate2 cert = store.Certificates.Cast<X509Certificate2>().First(c => c.Subject == "CN=*.mojang.com");
			File.WriteAllBytes("mojang.cer", cert.Export(X509ContentType.Cert));
			File.Copy(Path.Combine(Path.GetDirectoryName(thisExe), "decentralized-yggdrasil.jar"), "decentralized-yggdrasil.jar", true);
			File.Copy(Path.Combine(Path.GetDirectoryName(thisExe), "batch-launcher.exe"), "batch-launcher.exe", true);
			Process mc = Process.Start("java", "-jar decentralized-yggdrasil.jar mojang.cer");
			mc.WaitForExit();
			try {
				WebRequest.Create("http://localhost:56195/cghzddy/unlock").GetResponse().Close();
			} catch (Exception ex) {
				Console.Error.WriteLine(ex);
			}
		}
	}
}

