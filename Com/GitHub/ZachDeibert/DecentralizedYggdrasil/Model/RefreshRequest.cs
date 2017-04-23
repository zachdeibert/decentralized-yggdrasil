using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class RefreshRequest {
		[JsonProperty("accessToken")]
		public string AccessToken;
		[JsonProperty("clientToken")]
		public string ClientId;
		[JsonProperty("selectedProfile")]
		public Profile Profile;
		[JsonProperty("requestUser")]
		public bool IncludeUser;
	}
}

