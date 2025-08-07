using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieAndSeriesScanner.Services.Api.ServiceProviders;
using MovieAndSeriesScanner.Services.Api.ServiceProviders.HttpServiceProviders;
using MovieAndSeriesScanner.Services.Api.Services;

namespace MovieAndSeriesScanner.Services.Api.Configuration
{
	public static class ServicesAppConfiguration
	{
		public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddSingleton<ICacheService, CacheService>();
			services.AddTransient<ITmdbHttpService, TmdbHttpService>();
			services.AddTransient<IMovieService, MovieService>();

			return services;
		}

		public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration config)
		{
			var tmdbSection = config.GetSection("TMDBAPI");
			services.AddHttpClient("TMDB", client =>
			{
				client.BaseAddress = new Uri(tmdbSection["BaseUrl"]);
				client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tmdbSection["BearerToken"]);
			});
		}
	}
}
