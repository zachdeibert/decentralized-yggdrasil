using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class InvalidateApi : IApi {
		private TransientStateData State;

		public Type ParamType {
			get {
				return typeof(InvalidateRequest);
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
			return uri.AbsolutePath == "/invalidate";
		}

		public object Run(object param, Uri uri) {
			InvalidateRequest req = (InvalidateRequest) param;
			State.Profiles.Where(p => p.AccessToken == req.AccessToken).ToList().ForEach(p => p.AccessToken = Guid.Empty);
			return null;
		}
	}
}

