using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlType("profile")]
	public class TransientProfileData {
		private static readonly TimeSpan MaxTextureLife = TimeSpan.FromDays(1);
		[XmlElement("id")]
		public Guid ProfileId;
		[XmlElement("accessToken")]
		public Guid AccessToken;
		[XmlElement("joinedServer")]
		public string JoinedServerHash;
		[XmlElement("decryptedKey")]
		public DecryptedPrivateKey PrivateKey;
		[XmlElement("proxyToken")]
		public Guid ProxiedAccessToken;
		[XmlElement("texture")]
		public Property Texture;

		public bool IsTextureExpired {
			get {
				if (Texture == null) {
					return true;
				} else {
					Textures textures = JsonConvert.DeserializeObject<Textures>(Texture.Value);
					DateTime update = new DateTime(textures.TimestampMillis * 10000L);
					return update > DateTime.Now + MaxTextureLife;
				}
			}
		}

		public Property GetTexture(RealYggdrasil yggdrasil) {
			if (IsTextureExpired) {
				TexturedProfile profile = yggdrasil.Request<TexturedProfile>(string.Concat("https://sessionserver.mojang.com/session/minecraft/profile/", new string(ProfileId.ToString().Where(c => c != '-').ToArray()), "?unsigned=false"), null);
				if (profile != null) {
					Texture = profile.Properties.FirstOrDefault(p => p.Key == "textures");
				}
			}
			return Texture;
		}

		public TransientProfileData() {
		}

		public TransientProfileData(Guid profileId) {
			ProfileId = profileId;
		}
	}
}

