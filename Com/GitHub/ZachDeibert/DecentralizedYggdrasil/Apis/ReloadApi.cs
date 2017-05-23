using System;
using System.Collections.Generic;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class ReloadApi : IApi {
		private YggdrasilServer Server;

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

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state, RealYggdrasil yggdrasil) {
			Server = server;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/cghzddy/reload";
		}

		public object Run(object param, Uri uri) {
			Server.Reload();
			return null;
		}
	}
}

