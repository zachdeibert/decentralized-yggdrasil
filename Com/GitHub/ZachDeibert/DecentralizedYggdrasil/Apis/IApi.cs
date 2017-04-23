using System;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public interface IApi {
		Type ParamType {
			get;
		}

		string Method {
			get;
		}

		void Init(YggdrasilServer server);

		bool IsAcceptable(Uri uri);

		object Run(object param, Uri uri);
	}
}

