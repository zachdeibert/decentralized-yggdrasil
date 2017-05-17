using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class InvalidateApi : IApi {
		private TransientStateData State;
		private RealYggdrasil Yggdrasil;

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

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state, RealYggdrasil yggdrasil) {
			State = state;
			Yggdrasil = yggdrasil;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/invalidate";
		}

		public object Run(object param, Uri uri) {
			InvalidateRequest req = (InvalidateRequest) param;
			State.Profiles.Where(p => p.AccessToken == req.AccessToken).ToList().ForEach(p => {
				if (p.ProxiedAccessToken != Guid.Empty) {
					Yggdrasil.Request<object>("https://authserver.mojang.com/invalidate", new InvalidateRequest() {
						AccessToken = p.ProxiedAccessToken,
						ClientId = State.ProxyingClientId
					});
					p.ProxiedAccessToken = Guid.Empty;
				}
				p.AccessToken = Guid.Empty;
			});
			return null;
		}
	}
}

