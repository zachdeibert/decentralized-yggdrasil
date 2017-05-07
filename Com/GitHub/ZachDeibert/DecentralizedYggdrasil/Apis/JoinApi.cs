using System;
using System.Collections.Generic;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class JoinApi : IApi {
		public Type ParamType {
			get {
				return typeof(JoinServerRequest);
			}
		}

		public string Method {
			get {
				return "POST";
			}
		}

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state) {
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/session/minecraft/join";
		}

		public object Run(object param, Uri uri) {
			return null;
		}
	}
}

