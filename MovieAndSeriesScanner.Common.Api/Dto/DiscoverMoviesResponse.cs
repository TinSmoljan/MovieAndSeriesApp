using System.Collections.Generic;

namespace MovieAndSeriesScanner.Common.Api.Dto
{
    public class DiscoverMoviesResponse : TmdbMoviesApiResponse
    {
        public new List<MovieWithVideoResult> results { get; set; }
    }

    public class MovieWithVideoResult : TmdbMovieApiResponse
    {
        public string? trailerUrl { get; set; }

        public MovieWithVideoResult()
        {}

        public MovieWithVideoResult(TmdbMovieApiResponse movie)
        {
            this.adult = movie.adult;
            this.backdrop_path = movie.backdrop_path;
            this.id = movie.id;
            this.original_language = movie.original_language;
            this.original_title = movie.original_title;
            this.overview = movie.overview;
            this.popularity = movie.popularity;
            this.poster_path = movie.poster_path;
            this.release_date = movie.release_date;
            this.title = movie.title;
            this.video = movie.video;
            this.vote_average = movie.vote_average;
            this.vote_count = movie.vote_count;
            this.overview = movie.overview;
			this.belongs_to_collection = movie.belongs_to_collection;
			this.budget = movie.budget;
			this.homepage = movie.homepage;
			this.imdb_id = movie.imdb_id;
			this.origin_country = movie.origin_country;
			this.production_companies = movie.production_companies;
			this.production_countries = movie.production_countries;
			this.revenue = movie.revenue;
			this.runtime = movie.runtime;
			this.spoken_languages = movie.spoken_languages;
			this.status = movie.status;
			this.tagline = movie.tagline;
			this.genres = movie.genres;
        }
    }
}
