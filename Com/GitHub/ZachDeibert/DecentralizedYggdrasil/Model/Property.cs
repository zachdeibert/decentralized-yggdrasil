using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlType("property")]
	public class Property {
		[JsonProperty("name"), XmlElement("key")]
		public string Key;
		[JsonProperty("value"), XmlElement("value")]
		public string Value;
		[JsonProperty("signature"), XmlElement("signature")]
		public string Signature;

		public Property() {
		}

		public Property(string key, string value) {
			Key = key;
			Value = value;
		}
	}
}

