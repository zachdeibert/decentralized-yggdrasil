using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	class MainClass {
		public static void Main(string[] args) {
			string envvar = Environment.GetEnvironmentVariable("CGHZDDY_DIR");
			if (envvar == null) {
				string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "zachdeibert", "decentralized-yggdrasil");
				Directory.CreateDirectory(dir);
				Environment.CurrentDirectory = dir;
				Environment.SetEnvironmentVariable("CGHZDDY_DIR", dir);
			} else {
				Environment.CurrentDirectory = envvar;
			}
			string cmd = args.Length >= 1 ? args[0] : null;
			string[] cmdArgs = args.Skip(1).ToArray();
			foreach (Type t in Assembly.GetExecutingAssembly().GetTypes()) {
				if (typeof(ICommand).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface) {
					ICommand c = (ICommand) t.GetConstructor(new Type[0]).Invoke(new object[0]);
					if (cmd == c.Name) {
						c.Run(cmdArgs);
						return;
					}
				}
			}
		}
	}
}
