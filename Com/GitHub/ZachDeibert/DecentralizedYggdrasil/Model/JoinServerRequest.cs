using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class JoinServerRequest {
		[JsonProperty("accessToken")]
		public Guid AccessToken;
		[JsonProperty("selectedProfile")]
		public Guid ProfileId;
		[JsonProperty("serverId")]
		public string ServerHash;
	}
}

