using System;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	class MainClass {
		public static void Main(string[] args) {
			YggdrasilServer server = new YggdrasilServer(8080);
			server.Start();
			Console.WriteLine("Press any key to stop the server");
			Console.ReadKey(true);
			server.Stop();
		}
	}
}
