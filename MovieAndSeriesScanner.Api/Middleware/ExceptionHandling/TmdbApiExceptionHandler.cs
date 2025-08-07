using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MovieAndSeriesScanner.Common.Api.Exceptions;

namespace MovieAndSeriesScanner.Api.Middleware.ExceptionHandling
{
	internal sealed class TmdbApiExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<TmdbApiExceptionHandler> _logger;

		public TmdbApiExceptionHandler(ILogger<TmdbApiExceptionHandler> logger)
		{
			_logger = logger;
		}

		public async ValueTask<bool> TryHandleAsync(
			HttpContext httpContext,
			Exception exception,
			CancellationToken cancellationToken)
		{
			if (exception is not TmdbApiException tmdbException)
			{
				return false;
			}

			_logger.LogError(
				tmdbException,
				"tmdbException occurred from the tmdbApi: {StatusCode} {Message}",
				tmdbException.StatusCode,
				tmdbException.Message);

			string detail;
			switch (tmdbException.StatusCode)
			{
				case System.Net.HttpStatusCode.BadRequest:
					detail = "Bad Request, please check your input parameters.";
					break;
				default:
					detail = "A server side error occurred while processing your request.";
					break;
			}

			var problemDetails = new ProblemDetails
			{
				Status = (int)tmdbException.StatusCode,
				Title = "Bad Request",
				Detail = detail
			};

			httpContext.Response.StatusCode = problemDetails.Status.Value;

			await httpContext.Response
				.WriteAsJsonAsync(problemDetails, cancellationToken);

			return true;
		}
	}
}
