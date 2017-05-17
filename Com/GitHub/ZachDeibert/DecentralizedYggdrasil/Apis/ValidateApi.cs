using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class ValidateApi : IApi {
		private TransientStateData State;

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

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state) {
			State = state;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/validate";
		}

		public object Run(object param, Uri uri) {
			ValidateRequest req = (ValidateRequest) param;
			if (req.AccessToken == Guid.Empty || !State.Profiles.Any(p => p.AccessToken == req.AccessToken)) {
				throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
			}
			return null;
		}
	}
}

