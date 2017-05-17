using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class JoinServerRequest {
		[JsonProperty("accessToken"), JsonConverter(typeof(DashlessGuidConverter))]
		public Guid AccessToken;
		[JsonProperty("selectedProfile"), JsonConverter(typeof(DashlessGuidConverter))]
		public Guid ProfileId;
		[JsonProperty("serverId")]
		public string ServerHash;
	}
}

