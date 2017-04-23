using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class LoginRequest {
		[JsonProperty("agent")]
		public Agent Agent;
		[JsonProperty("username")]
		public string Email;
		[JsonProperty("password")]
		public string Password;
		[JsonProperty("clientToken")]
		public Guid ClientId;
		[JsonProperty("requestUser")]
		public bool IncludeUser;
	}
}

