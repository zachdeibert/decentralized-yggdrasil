using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class RefreshApi : IApi {
		private List<UserData> Users;
		private TransientStateData State;

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

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state) {
			Users = users;
			State = state;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/refresh";
		}

		public object Run(object param, Uri uri) {
			RefreshRequest req = (RefreshRequest) param;
			Guid profileId = State.AccessTokens.Where(p => p.Key == req.AccessToken).Select(p => p.Value).FirstOrDefault();
			if (profileId == default(Guid)) {
				throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
			} else {
				State.AccessTokens.Remove(new Pair<Guid, Guid>(req.AccessToken, profileId));
				Guid newToken = Guid.NewGuid();
				State.AccessTokens.Add(new Pair<Guid, Guid>(newToken, profileId));
				UserData user = Users.First(u => u.Profiles.Any(p => p.Id == profileId));
				Profile profile = user.Profiles.First(p => p.Id == profileId);
				return new AuthenticationResponse(newToken, req.ClientId, req.IncludeUser ? user.User : null, profile);
			}
		}
	}
}

