using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Commands {
	public class CloneCommand : ICommand {
		public string Name {
			get {
				return "clone";
			}
		}

		public void Run(string[] args) {
			if (args.Length == 2) {
				RealYggdrasil yggdrasil = new RealYggdrasil();
				TransientStateData state = TransientStateData.Load();
				AuthenticationResponse res = yggdrasil.Request<AuthenticationResponse>("https://authserver.mojang.com/authenticate", new LoginRequest() {
					Agent = Agent.Minecraft,
					Email = args[0],
					Password = args[1],
					ClientId = state.ProxyingClientId,
					IncludeUser = true
				});
				if (res == null) {
					Console.Error.WriteLine("Invalid credentials");
				} else {
					UserDataList users = UserDataList.Load();
					UserData user = new UserData();
					user.Email = args[0];
					user.Profiles.AddRange(res.Profiles);
					user.DefaultProfiles.Add(new Pair<Agent, Profile>(Agent.Minecraft, res.Profile));
					user.InitEncryption(args[1]);
					user.TryProxy = true;
					users.Add(user);
					UserDataList.Save(users);
				}
				TransientStateData.Save(state);
			} else {
				Console.Error.WriteLine("Usage: decentralized-yggdrasil.exe clone [email] [password]");
			}
		}
	}
}

