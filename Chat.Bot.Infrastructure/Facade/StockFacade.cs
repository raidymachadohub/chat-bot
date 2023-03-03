using System.Net;
using Chat.Bot.Infrastructure.Facade.Interfaces;

namespace Chat.Bot.Infrastructure.Facade
{
    public class StockFacade : IStockFacade
    {
        private readonly HttpClient _httpClient;

        public StockFacade(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("StockClient");
        }

        public async Task<string> GetStockAsync(string stockCode)
        {
            var resource = $"q/l/?s={stockCode.ToLower().Replace("/stock=","")}&f=sd2t2ohlcv&h&e=csv";
            using var httpClient = await _httpClient.GetAsync(resource);

            if (!httpClient.IsSuccessStatusCode)
                throw new Exception(ConvertStatusCode(httpClient));
            
            var responseStock = await httpClient.Content.ReadAsStringAsync();
            
            return responseStock;
        }

        private static string ConvertStatusCode(HttpResponseMessage? httpResponse) => httpResponse?.StatusCode switch
        {
            HttpStatusCode.BadRequest => $"Bad Request: {httpResponse.Content.ReadAsStringAsync().Result}",
            HttpStatusCode.NotFound => $"Not Found: {httpResponse.Content.ReadAsStringAsync().Result}",
            HttpStatusCode.Forbidden => $"Forbidden: {httpResponse.Content.ReadAsStringAsync().Result}",
            HttpStatusCode.InternalServerError => $"Internal Error: {httpResponse.Content.ReadAsStringAsync().Result}",
            _ => $"Error Undefined: {httpResponse?.Content.ReadAsStringAsync().Result}"
        };
    }
}