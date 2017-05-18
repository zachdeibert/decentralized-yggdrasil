using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class ValidateApi : IApi {
		private TransientStateData State;
		private RealYggdrasil Yggdrasil;

		public Type ParamType {
			get {
				return typeof(ValidateRequest);
			}
		}

		public string Method {
			get {
				return "POST";
			}
		}

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state, RealYggdrasil yggdrasil) {
			State = state;
			Yggdrasil = yggdrasil;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/validate";
		}

		public object Run(object param, Uri uri) {
			ValidateRequest req = (ValidateRequest) param;
			if (req.AccessToken == Guid.Empty || !State.Profiles.Any(p => p.AccessToken == req.AccessToken)) {
				throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
			}
			State.Profiles.Where(p => p.AccessToken == req.AccessToken && p.ProxiedAccessToken != Guid.Empty).ToList().ForEach(p => {
				HttpStatusCode status;
				Yggdrasil.Request<object>("https://authserver.mojang.com/validate", new ValidateRequest() {
					AccessToken = p.ProxiedAccessToken,
					ClientId = State.ProxyingClientId
				}, out status);
				if (status != HttpStatusCode.NoContent) {
					throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
				}
			});
			return null;
		}
	}
}

