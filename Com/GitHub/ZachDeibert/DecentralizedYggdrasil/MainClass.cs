using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	class MainClass {
		public static void Main(string[] args) {
			YggdrasilServer server = new YggdrasilServer(56195);
			server.Start();
			Process ssl;
			if (Type.GetType("Mono.Runtime") == null) {
				ssl = Process.Start("ssl-endpoint.exe", "*.mojang.com 443 localhost 56195");
			} else {
				ssl = Process.Start("mono", "ssl-endpoint.exe *.mojang.com 443 localhost 56195");
			}
			Thread.Sleep(1000);
			X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadOnly);
			X509Certificate2 cert = store.Certificates.Cast<X509Certificate2>().First(c => c.Subject == "CN=*.mojang.com");
			File.WriteAllBytes("mojang.cer", cert.Export(X509ContentType.Cert));
			Process mc = Process.Start("java", "-jar decentralized-yggdrasil.jar mojang.cer");
			mc.WaitForExit();
			server.Stop();
			ssl.Kill();
		}
	}
}
