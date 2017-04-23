using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class SignOutRequest {
		[JsonProperty("username")]
		public string Email;
		[JsonProperty("password")]
		public string Password;
	}
}

