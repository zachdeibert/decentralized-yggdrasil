using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlType("profile")]
	public class Profile {
		[JsonProperty("id"), XmlElement("id")]
		public Guid Id;
		[JsonProperty("name"), XmlElement("name")]
		public string Name;
		[JsonProperty("legacy"), XmlElement("legacy")]
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

