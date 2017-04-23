using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class UserProfile : User {
		[JsonProperty("name")]
		public string Name;

		public UserProfile() {
		}

		public UserProfile(Guid id, string name) : base(id) {
			Name = name;
		}

		public UserProfile(Guid id, string name, params Property[] properties) : base(id, properties) {
			Name = name;
		}
	}
}

