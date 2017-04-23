using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class AuthenticationResponse {
		[JsonProperty("accessToken")]
		public string AccessToken;
		[JsonProperty("clientToken")]
		public string ClientId;
		[JsonProperty("availableProfiles")]
		public List<Profile> Profiles;
		[JsonProperty("selectedProfile")]
		public Profile Profile;
		[JsonProperty("user")]
		public User User;

		public AuthenticationResponse() {
			Profiles = new List<Profile>();
		}

		public AuthenticationResponse(string accessToken, string clientId, Profile profile, params Profile[] profiles) : this() {
			AccessToken = accessToken;
			ClientId = clientId;
			Profiles.AddRange(profiles);
			Profile = profile;
		}

		public AuthenticationResponse(string accessToken, string clientId, Profile profile) : this(accessToken, clientId, profile, profile) {
		}

		public AuthenticationResponse(string accessToken, string clientId, User user, Profile profile, params Profile[] profiles) : this(accessToken, clientId, profile, profiles) {
			User = user;
		}

		public AuthenticationResponse(string accessToken, string clientId, User user, Profile profile) : this(accessToken, clientId, user, profile, profile) {
		}
	}
}

