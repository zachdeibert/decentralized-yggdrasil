using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class InvalidateRequest {
		[JsonProperty("accessToken")]
		public Guid AccessToken;
		[JsonProperty("clientToken")]
		public Guid ClientId;
	}
}

