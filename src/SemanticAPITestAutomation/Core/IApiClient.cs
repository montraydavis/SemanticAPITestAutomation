namespace SemanticAPITestAutomation.Core
{
    using System.Threading.Tasks;


    public interface IApiClient
    {
        Task<string> GetDataAsync(string endpoint);
    }

    public interface IDataProcessor
    {
        string ProcessData(string rawData);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<string> GetDataAsync(string endpoint)
        {
            HttpResponseMessage response = await this._httpClient.GetAsync(endpoint);
            return await response.Content.ReadAsStringAsync();
        }
    }

    public class DataProcessor : IDataProcessor
    {
        public string ProcessData(string rawData)
        {
            return rawData.ToUpperInvariant();
        }
    }
}
