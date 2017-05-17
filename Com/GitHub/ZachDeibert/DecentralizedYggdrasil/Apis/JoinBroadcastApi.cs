using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Apis {
	public class JoinBroadcastApi : IApi {
		private TransientStateData State;

		public Type ParamType {
			get {
				return typeof(JoinedServer);
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
			return uri.AbsolutePath == "/cghzddy/joined";
		}

		public static void OnJoined(JoinedServer record, TransientStateData state) {
			TransientProfileData data = state[record.ProfileId];
			if (data.JoinedServerHash != record.EncryptedHash) {
				data.JoinedServerHash = record.EncryptedHash;
				foreach (string server in state.UpstreamServers) {
					Task.Run(() => {
						try {
							WebRequest.Create(string.Concat(server, "/cghzddy/joined")).GetResponse().Close();
						} catch {
						}
					});
				}
			}
		}

		public object Run(object param, Uri uri) {
			JoinedServer req = (JoinedServer) param;
			OnJoined(req, State);
			return null;
		}
	}
}

