using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class User {
		[JsonProperty("id")]
		public string Id;
		[JsonProperty("properties")]
		public List<Property> Properties;

		public User() {
			Properties = new List<Property>();
		}

		public User(string id) : this() {
			Id = id;
		}

		public User(string id, params Property[] properties) : this(id) {
			Properties.AddRange(properties);
		}
	}
}

