using System;
using System.Xml.Serialization;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	[XmlType("pair")]
	public class Pair<TKey, TValue> {
		[XmlElement("key")]
		public TKey Key;
		[XmlElement("value")]
		public TValue Value;

		public Pair() {
		}

		public Pair(TKey key, TValue value) {
			Key = key;
			Value = value;
		}
	}
}

