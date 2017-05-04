using System;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class ChangePasswordCommand : ICommand {
		public string Name {
			get {
				return "changePass";
			}
		}

		public void Run(string[] args) {
			if (args.Length == 3) {
				UserDataList users = UserDataList.Load();
				UserData user = users.FirstOrDefault(u => u.Email == args[0]);
				if (user == null) {
					Console.Error.WriteLine("User not found.");
				} else if (user.ChangePassword(args[1], args[2])) {
					UserDataList.Save(users);
					Console.WriteLine("Password changed.");
				} else {
					Console.WriteLine("Invalid password.");
				}

			} else {
				Console.Error.WriteLine("Usage: decentralized-yggdrasil.exe checkPass [email] [old password] [new password]");
			}
		}
	}
}

