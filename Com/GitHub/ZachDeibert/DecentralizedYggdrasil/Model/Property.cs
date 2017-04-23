using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class Property {
		[JsonProperty("name")]
		public string Key;
		[JsonProperty("value")]
		public string Value;

		public Property() {
		}

		public Property(string key, string value) {
			Key = key;
			Value = value;
		}
	}
}

