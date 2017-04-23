using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class LoginApi : IApi {
		public Type ParamType {
			get {
				return typeof(LoginRequest);
			}
		}

		public string Method {
			get {
				return "POST";
			}
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/authenticate";
		}

		public object Run(object param, Uri uri) {
			LoginRequest req = (LoginRequest) param;
			return new AuthenticationResponse("deadbeef", req.ClientId, req.IncludeUser ? new User("deadbeef") : null, new Profile("deadbeef", "Zach"));
		}
	}
}

