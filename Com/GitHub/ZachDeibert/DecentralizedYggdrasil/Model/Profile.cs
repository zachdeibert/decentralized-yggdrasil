using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class Profile {
		[JsonProperty("id")]
		public string Id;
		[JsonProperty("name")]
		public string Name;
		[JsonProperty("legacy")]
		public bool IsLegacy;

		public Profile() {
		}

		public Profile(string id, string name, bool isLegacy) {
			Id = id;
			Name = name;
			IsLegacy = isLegacy;
		}

		public Profile(string id, string name) {
			Id = id;
			Name = name;
		}
	}
}

