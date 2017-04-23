using System;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public class StandardErrorException : Exception {
		public readonly Error Error;
		public readonly int StatusCode;
		public readonly string StatusDescription;

		public StandardErrorException(Error error, int statusCode, string statusDescription) : base(error.LongDesc) {
			Error = error;
			StatusCode = statusCode;
			StatusDescription = statusDescription;
		}
	}
}

