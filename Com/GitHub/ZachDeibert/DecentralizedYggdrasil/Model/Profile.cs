using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class Profile {
		[JsonProperty("id")]
		public Guid Id;
		[JsonProperty("name")]
		public string Name;
		[JsonProperty("legacy")]
		public bool IsLegacy;

		public Profile() {
		}

		public Profile(Guid id, string name, bool isLegacy) {
			Id = id;
			Name = name;
			IsLegacy = isLegacy;
		}

		public Profile(Guid id, string name) {
			Id = id;
			Name = name;
		}
	}
}

