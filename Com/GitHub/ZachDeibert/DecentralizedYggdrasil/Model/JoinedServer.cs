using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlType("joinData")]
	public class JoinedServer {
		[JsonProperty("profile"), XmlElement("profile")]
		public Guid ProfileId;
		[JsonProperty("hash"), XmlElement("hash")]
		public string EncryptedHash;

		public JoinedServer() {
		}

		public JoinedServer(Guid profileId, string hash) {
			ProfileId = profileId;
			EncryptedHash = hash;
		}
	}
}

