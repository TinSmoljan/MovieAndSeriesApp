namespace MovieAndSeriesScanner.Common.Api.Dto
{
    public class DiscoverMoviesParametersRequest
    {
		public string? query { get; set; }
		public int? page { get; set; } = 1;
        public string sort_by { get; set; } = "popularity.desc";
        public string language { get; set; } = "en-US";
        public bool? include_adult { get; set; } = false;
        public bool? include_video { get; set; } = false;
    }
}
