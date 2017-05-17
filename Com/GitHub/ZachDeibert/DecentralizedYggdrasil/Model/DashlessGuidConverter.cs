using System;
using System.Linq;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class DashlessGuidConverter : JsonConverter {
		public override bool CanConvert(Type objectType) {
			return objectType == typeof(Guid);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			return Guid.Parse(serializer.Deserialize<string>(reader));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			serializer.Serialize(writer, new string(value.ToString().Where(c => c != '-').ToArray()));
		}
	}
}

