using System;
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

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/invalidate";
		}

		public object Run(object param, Uri uri) {
			InvalidateRequest req = (InvalidateRequest) param;
			return null;
		}
	}
}

