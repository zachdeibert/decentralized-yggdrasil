using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class CheckJoinApi : IApi {
		private List<UserData> Users;

		public Type ParamType {
			get {
				return null;
			}
		}

		public string Method {
			get {
				return "GET";
			}
		}

		public void Init(YggdrasilServer server, List<UserData> users) {
			Users = users;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/session/minecraft/hasJoined";
		}

		public object Run(object param, Uri uri) {
			string email = uri.Query.Split('&').First(q => q.StartsWith("username=")).Split('=')[1];
			UserData user = Users.First(u => u.Email == email);
			return user.Profiles.First();
		}
	}
}

