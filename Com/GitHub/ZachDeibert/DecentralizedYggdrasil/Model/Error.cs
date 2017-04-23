using System;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class Error {
		[JsonProperty("error")]
		public string ShortDesc;
		[JsonProperty("errorMessage")]
		public string LongDesc;
		[JsonProperty("cause")]
		public string Cause;

		public Error(string shortDesc, string longDesc, string cause) {
			ShortDesc = shortDesc;
			LongDesc = longDesc;
			Cause = cause;
		}

		public Error(string shortDesc, string longDesc) : this(shortDesc, longDesc, null) {
		}

		public Error(Exception ex) : this(ex.GetType().Name, ex.Message, ex.InnerException == null ? null : ex.InnerException.Message) {
		}

		public Error() : this("Unknown error", "An error was constructed but not initialized") {
		}
	}
}

