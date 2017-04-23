using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class RefreshRequest {
		[JsonProperty("accessToken")]
		public Guid AccessToken;
		[JsonProperty("clientToken")]
		public Guid ClientId;
		[JsonProperty("selectedProfile")]
		public Profile Profile;
		[JsonProperty("requestUser")]
		public bool IncludeUser;
	}
}

