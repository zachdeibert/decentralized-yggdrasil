﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlRoot("data")]
	public class TransientStateData {
		private const string FileName = "transient.xml";
		[XmlArray("accessTokens")]
		public List<Pair<Guid, Guid>> AccessTokens;

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
			using (Stream stream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write)) {
				ser.Serialize(stream, data);
			}
		}

		public TransientStateData() {
			AccessTokens = new List<Pair<Guid, Guid>>();
		}
	}
}

