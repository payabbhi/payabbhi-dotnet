using System.Net;

namespace Payabbhi.Error
{
	public static class ErrorFactory
	{
		/// <summary>
		/// Create error entity from error message, error field, response json and status code of response.
		/// </summary>
		/// <returns>PayabbhiError</returns>
		/// <param name="message">Message.</param>
		/// <param name="field">Field.</param>
		/// <param name="payabbhiResponse">Payabbhi response.</param>
		/// <param name="httpStatusCode">Http status code.</param>
		public static BaseError Create(string message, string field, PayabbhiResponse payabbhiResponse, HttpStatusCode httpStatusCode)
		{
			switch (httpStatusCode)
			{
				case HttpStatusCode.BadRequest:
				case HttpStatusCode.NotFound:
					return new InvalidRequestError(message, field, payabbhiResponse, httpStatusCode);
				case HttpStatusCode.Unauthorized:
					return new AuthenticationError(message, field, payabbhiResponse, httpStatusCode);
				case HttpStatusCode.InternalServerError:
					return new ApiError(message, field, payabbhiResponse, httpStatusCode);
				case HttpStatusCode.BadGateway:
					if (string.IsNullOrWhiteSpace(message))
					{
						return new ApiError(Constants.Messages.ApiError, null, payabbhiResponse, httpStatusCode);
					}
					return new GatewayError(message, field, payabbhiResponse, httpStatusCode);
				default:
					return new ApiError("Unexpected HTTP code: " + httpStatusCode, null, payabbhiResponse, httpStatusCode);
			}
		}
	}
}
