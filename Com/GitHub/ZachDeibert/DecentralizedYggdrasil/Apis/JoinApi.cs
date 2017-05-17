﻿using System;
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
			TransientProfileData data = State[req.ProfileId];
			if (data.AccessToken != req.AccessToken) {
				throw new StandardErrorException(StandardErrors.InvalidToken, 403, "Forbidden");
			} else {
				RSACryptoServiceProvider rsa = data.PrivateKey.Key;
				JoinedServer server = new JoinedServer(req.ProfileId, Convert.ToBase64String(rsa.SignHash(req.ServerHash.SHA1HexToBytes(), CryptoConfig.MapNameToOID("SHA1"))));
				JoinBroadcastApi.OnJoined(server, State);
			}
			return null;
		}
	}
}

