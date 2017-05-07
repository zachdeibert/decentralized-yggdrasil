using System;
using System.Linq;
using System.Security.Cryptography;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil {
	public static class CryptoExtensions {
		public static string ToCamelXmlString(this RSA rsa, bool includePrivateParameters) {
			return rsa.ToXmlString(includePrivateParameters).Replace("Modulus>", "modulus>")
															.Replace("Exponent>", "exponent>")
															.Replace("InverseQ>", "inverseQ>");
		}

		public static void FromCamelXmlString(this RSA rsa, string xmlString) {
			rsa.FromXmlString(xmlString.Replace("modulus>", "Modulus>")
									   .Replace("exponent>", "Exponent>")
									   .Replace("inverseQ>", "InverseQ>"));
		}

		public static byte[] SHA1HexToBytes(this string hex) {
			bool isNegative = hex[0] == '-';
			if (isNegative) {
				hex = hex.Substring(1);
			}
			hex = new string(Enumerable.Repeat('0', 40 - hex.Length).Concat(hex).ToArray());
			byte[] bytes = Enumerable.Range(0, 20).Select(i => Convert.ToByte(hex.Substring(i * 2, 2), 16)).ToArray();
			if (isNegative) {
				for (int i = 0; i < 20; ++i) {
					bytes[i] = (byte) ~bytes[i];
				}
			}
			return bytes;
		}
	}
}

