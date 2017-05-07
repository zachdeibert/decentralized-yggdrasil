using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlRoot("users")]
	public class UserDataList : List<UserData> {
		private const string FileName = "users.xml";

		public static UserDataList Load() {
			if (File.Exists(FileName)) {
				XmlSerializer ser = new XmlSerializer(typeof(UserDataList));
				using (Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read)) {
					return (UserDataList) ser.Deserialize(stream);
				}
			} else {
				return new UserDataList();
			}
		}

		public static void Save(UserDataList users) {
			XmlSerializer ser = new XmlSerializer(typeof(UserDataList));
			using (Stream stream = File.Create(FileName)) {
				ser.Serialize(stream, users);
			}
		}
	}
}

