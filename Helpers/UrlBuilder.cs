namespace StarWarsAPI.Helpers
{
    public static class UrlBuilder
    {
        public static string BuildStarshipUrl(HttpContext context, int id)
        {
            var request = context.Request;
            var baseUrl = $"{request.Scheme}://{request.Host.Value}{request.PathBase}";

            return $"{baseUrl}/api/starships/{id}";
        }
    }

}
