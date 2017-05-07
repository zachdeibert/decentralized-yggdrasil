using System;

namespace Com.GitHub.ZachDeibert.DecentralizedYggdrasil.Model {
	public static class StandardErrors {
		public static readonly Error InvalidMethod = new Error("Method Not Allowed", "The method specified in the request is not allowed for the resource identified by the request URI");
		public static readonly Error InvalidURL = new Error("Not Found", "The server has not found anything matching the request URI");
		public static readonly Error MigratedAccount = new Error("ForbiddenOperationException", "Invalid credentials.  Account migrated, use e-mail as username.", "UserMigratedException");
		public static readonly Error InvalidCredentials = new Error("ForbiddenOperationException", "Invalid credentials.  Invalid username or password.");
		public static readonly Error TooManyLoginAttempts = new Error("ForbiddenOperationException", "Invalid credentials.");
		public static readonly Error InvalidToken = new Error("ForbiddenOperationException", "Invalid token.");
		public static readonly Error ProfileAlreadyAssigned = new Error("IllegalArgumentException", "Access token already has a profile assigned.");
		public static readonly Error NoCredentials = new Error("IllegalArgumentException", "credentials is null");
		public static readonly Error InvalidType = new Error("Unsupported Media Type", "The server is refusing to service the request because the entity of the request is in a format not supported by the requested resource for the requested method");
	}
}

