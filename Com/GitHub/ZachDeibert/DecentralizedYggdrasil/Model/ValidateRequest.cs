﻿using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class ValidateRequest {
		[JsonProperty("accessToken")]
		public string AccessToken;
		[JsonProperty("clientToken")]
		public string ClientId;
	}
}

