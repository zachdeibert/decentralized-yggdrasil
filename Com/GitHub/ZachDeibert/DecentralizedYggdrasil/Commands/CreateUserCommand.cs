using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class CreateUserCommand : ICommand {
		public string Name {
			get {
				return "create";
			}
		}

		public void Run(string[] args) {
			if (args.Length == 3) {
				UserDataList users = UserDataList.Load();
				UserData user = new UserData();
				user.Email = args[0];
				Profile profile = new Profile(Guid.NewGuid(), args[1]);
				user.Profiles.Add(profile);
				user.DefaultProfiles.Add(new Pair<Agent, Profile>(Agent.Minecraft, profile));
				user.InitEncryption(args[2]);
				users.Add(user);
				UserDataList.Save(users);
			} else {
				Console.Error.WriteLine("Usage: decentralized-yggdrasil.exe create [email] [name] [password]");
			}
		}
	}
}

