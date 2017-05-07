using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class JoinApi : IApi {
		private TransientStateData State;

		public Type ParamType {
			get {
				return typeof(JoinServerRequest);
			}
		}

		public string Method {
			get {
				return "POST";
			}
		}

		public void Init(YggdrasilServer server, List<UserData> users, TransientStateData state) {
			State = state;
		}

		public bool IsAcceptable(Uri uri) {
			return uri.AbsolutePath == "/session/minecraft/join";
		}

		public object Run(object param, Uri uri) {
			JoinServerRequest req = (JoinServerRequest) param;
			Guid profileId = State.AccessTokens.Where(p => p.Key == req.AccessToken).Select(p => p.Value).FirstOrDefault();
			if (profileId == default(Guid)) {
				throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
			} else if (profileId == req.ProfileId) {
				RSACryptoServiceProvider rsa = State.Keys.First(k => k.Id == req.AccessToken).Key;
				JoinedServer server = new JoinedServer(req.ProfileId, Convert.ToBase64String(rsa.SignHash(req.ServerHash.SHA1HexToBytes(), CryptoConfig.MapNameToOID("SHA1"))));
				JoinBroadcastApi.OnJoined(server, State);
			} else {
				throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
			}
			return null;
		}
	}
}

