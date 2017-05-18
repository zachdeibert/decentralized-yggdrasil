using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class TexturedProfile : Profile {
		[JsonProperty("properties")]
		public List<Property> Properties;

		public TexturedProfile() {
		}

		public TexturedProfile(Profile profile, Property texture) : base(profile.Id, profile.Name, profile.IsLegacy) {
			Properties = new List<Property>();
			Properties.Add(texture);
		}
	}
}

