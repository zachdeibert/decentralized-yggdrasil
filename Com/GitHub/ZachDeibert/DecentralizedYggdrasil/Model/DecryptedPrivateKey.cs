using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlType("decryptedKey")]
	public class DecryptedPrivateKey {
		[XmlElement("id")]
		public Guid Id;
		[XmlAnyElement("RSAKeyValue")]
		public XmlNode PrivateKey;

		public RSACryptoServiceProvider Key {
			get {
				StringBuilder str = new StringBuilder();
				using (XmlWriter writer = XmlWriter.Create(str)) {
					PrivateKey.WriteTo(writer);
				}
				RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
				rsa.FromCamelXmlString(str.ToString());
				return rsa;
			}
		}

		public DecryptedPrivateKey() {
		}

		public DecryptedPrivateKey(Guid id, RSA rsa) {
			Id = id;
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(rsa.ToCamelXmlString(true));
			PrivateKey = doc.DocumentElement;
		}
	}
}

