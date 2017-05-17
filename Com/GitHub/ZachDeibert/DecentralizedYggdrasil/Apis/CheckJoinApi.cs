using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class CheckJoinApi : IApi {
		private List<UserData> Users;
		private TransientStateData State;
		private RealYggdrasil Yggdrasil;

		public Type ParamType {
			get {
				return null;
			}
		}

		public string Method {
			get {
				return "GET";
			}
		}

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state, RealYggdrasil yggdrasil) {
			Users = users;
			State = state;
			Yggdrasil = yggdrasil;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/session/minecraft/hasJoined";
		}

		public object Run(object param, Uri uri) {
			string[] queries = uri.Query.Substring(1).Split('&');
			string username = queries.FirstOrDefault(q => q.StartsWith("username=")).Split('=')[1];
			string serverHash = queries.FirstOrDefault(q => q.StartsWith("serverId=")).Split('=')[1];
			if (username == null || serverHash == null) {
				throw new StandardErrorException(StandardErrors.InvalidURL, 400, "Bad Request");
			}
			IEnumerable<Profile> matchedProfiles = Users.SelectMany(u => u.Profiles).Where(p => p.Name == username);
			foreach (Profile profile in matchedProfiles) {
				TransientProfileData data = State[profile];
				UserData user = Users.First(u => u.Profiles.Any(p => p.Id == profile.Id));
				RSACryptoServiceProvider rsa = user.RSAPublicKey;
				if (rsa.VerifyHash(serverHash.SHA1HexToBytes(), CryptoConfig.MapNameToOID("SHA1"), Convert.FromBase64String(data.JoinedServerHash))) {
					return profile;
				} else if (user.TryProxy) {
					HttpStatusCode status;
					Profile real = Yggdrasil.Request<Profile>(string.Concat("https://sessionserver.mojang.com/session/minecraft/hasJoined?username=", username, "&serverId=", serverHash), null, out status);
					if (status == HttpStatusCode.OK) {
						return real;
					} else {
						throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
					}
				} else {
					throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
				}
			}
			throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
		}
	}
}

