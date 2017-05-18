using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class RefreshApi : IApi {
		private List<UserData> Users;
		private TransientStateData State;
		private RealYggdrasil Yggdrasil;

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

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state, RealYggdrasil yggdrasil) {
			Users = users;
			State = state;
			Yggdrasil = yggdrasil;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/refresh";
		}

		public object Run(object param, Uri uri) {
			RefreshRequest req = (RefreshRequest) param;
			TransientProfileData data = State.Profiles.FirstOrDefault(p => p.AccessToken == req.AccessToken);
			if (data == null) {
				throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
			} else {
				UserData user = Users.First(u => u.Profiles.Any(p => p.Id == data.ProfileId));
				Profile profile = user.Profiles.First(p => p.Id == data.ProfileId);
				if (data.ProxiedAccessToken != Guid.Empty) {
					AuthenticationResponse res = Yggdrasil.Request<AuthenticationResponse>("https://authserver.mojang.com/refresh", new RefreshRequest() {
						AccessToken = data.ProxiedAccessToken,
						ClientId = State.ProxyingClientId,
						Profile = req.Profile,
						IncludeUser = false
					});
					if (res == null) {
						throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
					} else {
						data.ProxiedAccessToken = res.AccessToken;
					}
				}
				Guid newToken = Guid.NewGuid();
				data.AccessToken = newToken;
				return new AuthenticationResponse(newToken, req.ClientId, req.IncludeUser ? user.User : null, profile);
			}
		}
	}
}

