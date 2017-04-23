using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class JoinApi : IApi {
		public Type ParamType {
			get {
				return typeof(JoinApi);
			}
		}

		public string Method {
			get {
				return "POST";
			}
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/session/minecraft/join";
		}

		public object Run(object param, Uri uri) {
			JoinApi req = (JoinApi) param;
			return null;
		}
	}
}

