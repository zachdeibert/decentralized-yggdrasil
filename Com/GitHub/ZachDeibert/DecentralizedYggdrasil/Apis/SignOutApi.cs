using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class SignOutApi : IApi {
		private List<UserData> Users;
		private TransientStateData State;
		private RealYggdrasil Yggdrasil;

		public Type ParamType {
			get {
				return typeof(SignOutRequest);
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
			return uri.AbsolutePath == "/signout";
		}

		public object Run(object param, Uri uri) {
			SignOutRequest req = (SignOutRequest) param;
			UserData user = Users.FirstOrDefault(u => u.Email == req.Email);
			if (user == null) {
				throw new StandardErrorException(StandardErrors.InvalidCredentials, 403, "Forbidden");
			} else if (user.GetPrivateKey(req.Password) == null) {
				throw new StandardErrorException(StandardErrors.InvalidCredentials, 403, "Forbidden");
			} else {
				State.Profiles.Where(p => user.Profiles.Any(p2 => p.ProfileId == p2.Id)).ToList().ForEach(p => p.AccessToken = Guid.Empty);
				if (user.TryProxy) {
					Yggdrasil.Request<object>("https://authserver.mojang.com/signout", req);
				}
			}
			return null;
		}
	}
}

