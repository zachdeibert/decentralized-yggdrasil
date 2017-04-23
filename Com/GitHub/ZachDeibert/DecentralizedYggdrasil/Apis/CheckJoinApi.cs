using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class CheckJoinApi : IApi {
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

		public void Init(YggdrasilServer server) {
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/session/minecraft/hasJoined";
		}

		public object Run(object param, Uri uri) {
			return new UserProfile(Guid.Empty, "Zach");
		}
	}
}

