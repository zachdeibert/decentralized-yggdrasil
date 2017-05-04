using System;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class CheckPasswordCommand : ICommand {
		public string Name {
			get {
				return "checkPass";
			}
		}

		public void Run(string[] args) {
			if (args.Length == 2) {
				UserDataList users = UserDataList.Load();
				UserData user = users.FirstOrDefault(u => u.Email == args[0]);
				if (user == null) {
					Console.Error.WriteLine("User not found.");
				} else if (user.GetPrivateKey(args[1]) == null) {
					Console.WriteLine("Invalid password.");
				} else {
					Console.WriteLine("Correct password.");
				}
			} else {
				Console.Error.WriteLine("Usage: decentralized-yggdrasil.exe checkPass [email] [password]");
			}
		}
	}
}

