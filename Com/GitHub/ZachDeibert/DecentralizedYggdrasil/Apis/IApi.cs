using System;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public interface IApi {
		Type ParamType {
			get;
		}

		string Method {
			get;
		}

		bool IsAcceptable(Uri uri);

		object Run(object param, Uri uri);
	}
}

