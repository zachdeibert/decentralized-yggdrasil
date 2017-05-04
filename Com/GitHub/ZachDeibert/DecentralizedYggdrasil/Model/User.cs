using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class User {
		[JsonProperty("id"), XmlElement("id")]
		public Guid Id;
		[JsonProperty("properties"), XmlArray("properties")]
		public List<Property> Properties;

		public User() : this(Guid.NewGuid()) {
		}

		public User(Guid id) {
			Properties = new List<Property>();
			Id = id;
		}

		public User(Guid id, params Property[] properties) : this(id) {
			Properties.AddRange(properties);
		}
	}
}

