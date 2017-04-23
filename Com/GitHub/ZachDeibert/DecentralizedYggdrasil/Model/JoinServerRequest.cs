using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class JoinServerRequest {
		[JsonProperty("accessToken")]
		public string AccessToken;
		[JsonProperty("selectedProfile")]
		public string ProfileId;
		[JsonProperty("serverId")]
		public string ServerHash;
	}
}

