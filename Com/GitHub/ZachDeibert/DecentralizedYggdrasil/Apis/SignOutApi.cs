using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class SignOutApi : IApi {
		public Type ParamType {
			get {
				return typeof(SignOutRequest);
			}
		}

		public string Method {
			get {
				return "POST";
			}
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/signout";
		}

		public object Run(object param, Uri uri) {
			SignOutRequest req = (SignOutRequest) param;
			return null;
		}
	}
}

