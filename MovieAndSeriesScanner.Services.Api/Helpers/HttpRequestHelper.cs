using System.Web;

namespace MovieAndSeriesScanner.Services.Api.Helpers
{
    public static class HttpRequestHelper
    {
        public static string BuildUrlWithQueryParams<T>(string url, T queryParams)
        {
            return AddQueryParamsFromObject(url, queryParams);
        }

        private static string AddQueryParamsFromObject<T>(string url, T queryParams)
        {
            if (queryParams == null)
                return url;

            var props = queryParams.GetType().GetProperties();
            var queryList = new List<string>();
            foreach (var prop in props)
            {
                var value = prop.GetValue(queryParams);
                if (value == null)
                    continue;
                string encodedName = HttpUtility.UrlEncode(prop.Name);
                string encodedValue = HttpUtility.UrlEncode(value.ToString());
                queryList.Add($"{encodedName}={encodedValue}");
            }
            string queryString = string.Join("&", queryList);
            if (string.IsNullOrEmpty(queryString))
                return url;

            if (url.EndsWith("?"))
                return url + "&" + queryString;
            else
                return url + "?" + queryString;
        }
    }
}
