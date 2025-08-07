using System.Net;

namespace MovieAndSeriesScanner.Common.Api.Exceptions
{
	public class TmdbApiException : Exception
	{
		public HttpStatusCode StatusCode { get; set; }
		public TmdbApiException(string message, HttpStatusCode status)
			: base(message)
		{
			StatusCode = status;
		}
	}
}
