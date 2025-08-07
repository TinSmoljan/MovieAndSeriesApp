
using Microsoft.Extensions.Configuration;
using MovieAndSeriesScanner.Common.Api.Dto;
using MovieAndSeriesScanner.Common.Api.Exceptions;
using MovieAndSeriesScanner.Common.Api.Models;
using MovieAndSeriesScanner.Services.Api.Helpers;
using MovieAndSeriesScanner.Services.Api.Services;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Net;

namespace MovieAndSeriesScanner.Services.Api.ServiceProviders.HttpServiceProviders
{
	public class TmdbHttpService : ITmdbHttpService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfigurationSection _tmdbConfig;
		private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

		public TmdbHttpService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClient = httpClientFactory.CreateClient("TMDB");
			_tmdbConfig = configuration.GetSection("TMDBAPI");

			_retryPolicy = Policy
				.HandleResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500 && (int)r.StatusCode < 600)
				.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
		}

		public async Task<TmdbMoviesApiResponse> GetMoviesAsync(DiscoverMoviesParametersRequest request)
		{
			var urlTemplate = string.IsNullOrEmpty(request.query) ? _tmdbConfig["DiscoverMoviesUrl"] : _tmdbConfig["SearchMovieUrl"];

			var result = await SendGetRequest<TmdbMoviesApiResponse, DiscoverMoviesParametersRequest>(urlTemplate, request);
			return result;
		}

		public async Task<TmdbMovieApiResponse> GetMovieDetailsAndVideoAsync(int movieId)
		{
			var urlTemplate = _tmdbConfig["MovieDetailsUrl"];
			var url = urlTemplate.Replace("{id}", movieId.ToString());
			return await SendGetRequest<TmdbMovieApiResponse, AppendToResponseModel>(url, new AppendToResponseModel("videos"));
		}

		private async Task<T> SendGetRequest<T, Z>(string requestUrl, Z? queryParams)
		{
			string finalUrl = requestUrl;
			if (queryParams != null)
			{
				finalUrl = HttpRequestHelper.BuildUrlWithQueryParams<Z>(requestUrl, queryParams);
			}

			var response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(finalUrl));
			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == HttpStatusCode.BadRequest)
				{
					throw new TmdbApiException("Bad request", response.StatusCode);
				}
				else
				{
					throw new TmdbApiException("Failed to GET from Tmdb API", response.StatusCode);
				}
			}

			var responseContent = await response.Content.ReadAsStringAsync();
			var apiResult = JsonConvert.DeserializeObject<T>(responseContent);

			return apiResult;
		}
	}
}
