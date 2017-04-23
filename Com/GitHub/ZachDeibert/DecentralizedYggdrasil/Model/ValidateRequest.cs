using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class ValidateRequest {
		[JsonProperty("accessToken")]
		public Guid AccessToken;
		[JsonProperty("clientToken")]
		public Guid ClientId;
	}
}

