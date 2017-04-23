using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class ValidateApi : IApi {
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

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/validate";
		}

		public object Run(object param, Uri uri) {
			ValidateRequest req = (ValidateRequest) param;
			return null;
		}
	}
}

