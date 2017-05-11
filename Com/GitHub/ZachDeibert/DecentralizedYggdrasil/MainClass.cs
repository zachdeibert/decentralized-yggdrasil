using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	class MainClass {
		public static void Main(string[] args) {
			UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
			Environment.CurrentDirectory = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
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
