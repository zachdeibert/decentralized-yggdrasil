﻿using System;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class RefreshApi : IApi {
		public Type ParamType {
			get {
				return typeof(RefreshRequest);
			}
		}

		public string Method {
			get {
				return "POST";
			}
		}

		public void Init(YggdrasilServer server) {
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/refresh";
		}

		public object Run(object param, Uri uri) {
			RefreshRequest req = (RefreshRequest) param;
			return new AuthenticationResponse(Guid.Empty, req.ClientId, req.IncludeUser ? new User(Guid.Empty) : null, new Profile(Guid.Empty, "Zach"));
		}
	}
}

