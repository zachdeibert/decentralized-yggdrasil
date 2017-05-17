using System;
using System.Xml.Serialization;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlType("profile")]
	public class TransientProfileData {
		[XmlElement("id")]
		public Guid ProfileId;
		[XmlElement("accessToken")]
		public Guid AccessToken;
		[XmlElement("joinedServer")]
		public string JoinedServerHash;
		[XmlElement("decryptedKey")]
		public DecryptedPrivateKey PrivateKey;

		public TransientProfileData() {
		}

		public TransientProfileData(Guid profileId) {
			ProfileId = profileId;
		}
	}
}

