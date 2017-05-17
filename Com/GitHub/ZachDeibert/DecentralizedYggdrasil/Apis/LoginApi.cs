using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class LoginApi : IApi {
		private List<UserData> Users;
		private TransientStateData State;

		public Type ParamType {
			get {
				return typeof(LoginRequest);
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
			return uri.AbsolutePath == "/authenticate";
		}

		public object Run(object param, Uri uri) {
			LoginRequest req = (LoginRequest) param;
			UserData user = Users.FirstOrDefault(u => u.Email == req.Email);
			RSA key;
			if (user == null) {
				throw new StandardErrorException(StandardErrors.InvalidCredentials, 403, "Forbidden");
			} else if ((key = user.GetPrivateKey(req.Password)) == null) {
				throw new StandardErrorException(StandardErrors.InvalidCredentials, 403, "Forbidden");
			} else {
				Guid token = Guid.NewGuid();
				Profile profile = (user.DefaultProfiles.FirstOrDefault(p => p.Key.Name == req.Agent.Name) ?? new Pair<Agent, Profile>(null, null)).Value;
				TransientProfileData data = State[profile];
				data.AccessToken = token;
				data.PrivateKey = new DecryptedPrivateKey(token, key);
				return new AuthenticationResponse(token, req.ClientId, req.IncludeUser ? user.User : null, profile);
			}
		}
	}
}

