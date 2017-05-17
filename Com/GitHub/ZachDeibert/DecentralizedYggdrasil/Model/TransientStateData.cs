using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlRoot("data")]
	public class TransientStateData {
		private const string FileName = "transient.xml";
		[XmlArray("profiles")]
		public List<TransientProfileData> Profiles;
		[XmlArray("upstream")]
		public List<string> UpstreamServers;
		[XmlElement("clientId")]
		public Guid ProxyingClientId;

		public TransientProfileData this[Guid profileId] {
			get {
				TransientProfileData profile = Profiles.FirstOrDefault(p => p.ProfileId == profileId);
				if (profile == null) {
					profile = new TransientProfileData(profileId);
					Profiles.Add(profile);
				}
				return profile;
			}
		}

		public TransientProfileData this[Profile profile] {
			get {
				return this[profile.Id];
			}
		}

		public static TransientStateData Load() {
			if (File.Exists(FileName)) {
				XmlSerializer ser = new XmlSerializer(typeof(TransientStateData));
				using (Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read)) {
					return (TransientStateData) ser.Deserialize(stream);
				}
			} else {
				return new TransientStateData();
			}
		}

		public static void Save(TransientStateData data) {
			XmlSerializer ser = new XmlSerializer(typeof(TransientStateData));
			using (Stream stream = File.Create(FileName)) {
				ser.Serialize(stream, data);
			}
		}

		public TransientStateData() {
			Profiles = new List<TransientProfileData>();
			UpstreamServers = new List<string>();
			ProxyingClientId = Guid.NewGuid();
		}
	}
}

