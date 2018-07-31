using System;
namespace Payabbhi
{
	public static class Constants
	{
		public static class Messages
		{
			public const string ApiConnectionError = @"Unexpected error communicating with Payabbhi. If this problem persists, let us know at support@payabbhi.com.";
			public const string ApiError = @"Something did not work as expected on our side";
			public const string InvalidArgumentError = @"The arguments provided are invalid.";
			public const string InvalidSignatureError = @"Invalid signature passed";
			public const string InvalidCallError = @"Object Id not set";
		}
	}
}
