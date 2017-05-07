using System;
using System.Collections.Generic;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public interface IApi {
		Type ParamType {
			get;
		}

		string Method {
			get;
		}

		void Init(YggdrasilServer server, List<UserData> users, TransientStateData state);

		bool IsAcceptable(Uri uri);

		object Run(object param, Uri uri);
	}
}

