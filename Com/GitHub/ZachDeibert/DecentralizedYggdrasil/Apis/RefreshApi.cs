using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class RefreshApi : IApi {
		public Type ParamType {
			get {
				return typeof(RefreshRequest);
			}
		}

		public string Method {
			get {
				return "POST";
			}
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/refresh";
		}

		public object Run(object param, Uri uri) {
			RefreshRequest req = (RefreshRequest) param;
			return new AuthenticationResponse("deadbeef", req.ClientId, req.IncludeUser ? new User("deadbeef") : null, new Profile("deadbeef", "Zach"));
		}
	}
}

