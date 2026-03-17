using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Shared.Infrastructure.ExternalServices
{
    public class ExternalApiClient : IExternalApiClient
    {
        private readonly HttpClient _httpClient;

        //private const string GetCompanyByIdEndpoint = "/api/Company/GetCompanyById/{0}";

        public ExternalApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        //public async Task<CompanyData?> GetCompanyByIdAsync(int companyId, string token)
        //{
        //    if (string.IsNullOrWhiteSpace(token))
        //        throw new ArgumentException("Token cannot be null or empty", nameof(token));

        //    // Set JWT Bearer
        //    _httpClient.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", token);

        //    // Gọi API external
        //    var url = string.Format(GetCompanyByIdEndpoint, companyId);

        //    try
        //    {
        //        var response = await _httpClient.GetAsync(url);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            // Có thể log status code hoặc message
        //            return null;
        //        }

        //        // Deserialize JSON về CompanyResponse
        //        var companyResponse = await response.Content.ReadFromJsonAsync<GetCompanyByIdResponse>();

        //        return companyResponse?.Data;
        //    }
        //    catch (HttpRequestException)
        //    {
        //        // Có thể log lỗi network
        //        return null;
        //    }
        //}
    }
}