using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class Textures {
		[JsonProperty("timestamp")]
		public long TimestampMillis;
		[JsonProperty("profileId")]
		public Guid ProfileId;
		[JsonProperty("profileName")]
		public string Name;
		[JsonProperty("isPublic")]
		public bool Public;
		[JsonProperty("textures")]
		public Dictionary<string, Texture> Data;
	}
}

