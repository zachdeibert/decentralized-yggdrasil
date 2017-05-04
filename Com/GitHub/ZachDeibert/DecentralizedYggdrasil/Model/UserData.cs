using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlType("userData")]
	public class UserData {
		[XmlElement("user")]
		public User User;
		[XmlArray("profiles")]
		public List<Profile> Profiles;
		[XmlArray("defaultProfiles")]
		public List<Pair<Agent, Profile>> DefaultProfiles;
		[XmlElement("email")]
		public string Email;
		[XmlElement("salt")]
		public string InitializationVector;
		[XmlElement("password")]
		public string EncryptedPassword;
		[XmlElement("private")]
		public string EncryptedPrivateKey;
		[XmlAnyElement("RSAKeyValue")]
		public XmlNode PublicKey;

		public void InitEncryption(string password) {
			Aes aes = Aes.Create();
			aes.GenerateIV();
			InitializationVector = Convert.ToBase64String(aes.IV);
			int length = Encoding.UTF8.GetByteCount(password);
			while (!aes.ValidKeySize(length * 8)) {
				++length;
			}
			byte[] buffer = new byte[length];
			Encoding.UTF8.GetBytes(password).CopyTo(buffer, 0);
			aes.Key = buffer;
			EncryptedPassword = EncryptString(aes, password);
			RSA rsa = RSA.Create();
			EncryptedPrivateKey = EncryptString(aes, rsa.ToXmlString(true));
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(rsa.ToXmlString(false));
			PublicKey = doc.DocumentElement;
		}

		private static string Encrypt(Aes aes, byte[] decrypted) {
			ICryptoTransform enc = aes.CreateEncryptor();
			return Convert.ToBase64String(enc.TransformFinalBlock(decrypted, 0, decrypted.Length));
		}

		private static string EncryptString(Aes aes, string decrypted) {
			return Encrypt(aes, Encoding.UTF8.GetBytes(decrypted));
		}

		private static byte[] Decrypt(Aes aes, string encrypted) {
			try {
				ICryptoTransform dec = aes.CreateDecryptor();
				byte[] data = Convert.FromBase64String(encrypted);
				return dec.TransformFinalBlock(data, 0, data.Length);
			} catch (CryptographicException) {
				return null;
			}
		}

		private static string DecryptString(Aes aes, string encrypted) {
			byte[] data = Decrypt(aes, encrypted);
			if (data == null) {
				return null;
			} else {
				return Encoding.UTF8.GetString(data);
			}
		}

		public RSA GetPrivateKey(string password) {
			Aes aes = Aes.Create();
			int length = Encoding.UTF8.GetByteCount(password);
			while (!aes.ValidKeySize(length * 8)) {
				++length;
			}
			byte[] buffer = new byte[length];
			Encoding.UTF8.GetBytes(password).CopyTo(buffer, 0);
			aes.IV = Convert.FromBase64String(InitializationVector);
			aes.Key = buffer;
			if (DecryptString(aes, EncryptedPassword) == password) {
				string key = DecryptString(aes, EncryptedPrivateKey);
				if (key == null) {
					return null;
				} else {
					RSA rsa = RSA.Create();
					rsa.FromXmlString(key);
					return rsa;
				}
			} else {
				return null;
			}
		}

		public bool ChangePassword(string oldPass, string newPass) {
			Aes aes = Aes.Create();
			int length = Encoding.UTF8.GetByteCount(oldPass);
			while (!aes.ValidKeySize(length * 8)) {
				++length;
			}
			byte[] buffer = new byte[length];
			Encoding.UTF8.GetBytes(oldPass).CopyTo(buffer, 0);
			aes.IV = Convert.FromBase64String(InitializationVector);
			aes.Key = buffer;
			byte[] privateKey = Decrypt(aes, EncryptedPrivateKey);
			if (privateKey == null) {
				return false;
			} else {
				length = Encoding.UTF8.GetByteCount(newPass);
				while (!aes.ValidKeySize(length * 8)) {
					++length;
				}
				buffer = new byte[length];
				Encoding.UTF8.GetBytes(newPass).CopyTo(buffer, 0);
				aes.Key = buffer;
				EncryptedPassword = EncryptString(aes, newPass);
				EncryptedPrivateKey = Encrypt(aes, privateKey);
				return true;
			}
		}

		public UserData() {
			Profiles = new List<Profile>();
			DefaultProfiles = new List<Pair<Agent, Profile>>();
		}
	}
}

