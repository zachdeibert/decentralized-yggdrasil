using System;
using System.Collections.Generic;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class InvalidateApi : IApi {
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

		public void Init(YggdrasilServer server, List<UserData> users) {
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/invalidate";
		}

		public object Run(object param, Uri uri) {
			return null;
		}
	}
}

