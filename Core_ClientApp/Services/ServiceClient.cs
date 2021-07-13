using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core_ClientApp.Services
{
	public class ServiceClient
    {
        private readonly IHttpClientFactory httpClient;
        private readonly ITokenAcquisition token;
        private readonly IConfiguration cfg;

        public ServiceClient(IHttpClientFactory client,
            ITokenAcquisition token,
            IConfiguration cfg)
        {
            this.httpClient = client;
            this.token = token;
            this.cfg = cfg;
        }

        public async Task<string> GetDataAsync()
        {
            try
            {
                // create HTTP Client
                var client = httpClient.CreateClient();
                // read api token
                var apiToken = cfg["ApiConfig:AccessToken"];
                // get access token for authenticated user
                var accessToken = await token.GetAccessTokenForUserAsync(new[] { apiToken });
                // read the service base adddress and define HTTP Request details
                client.BaseAddress = new Uri(cfg["ApiConfig:ApiBaseAddress"]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // make call to service
                var response = await client.GetAsync("weatherforecast");
                if (response.IsSuccessStatusCode)
                {
                    // generate response
                    var receivedData = await response.Content.ReadAsStringAsync();
                    return receivedData;
                }
                // if error occured then return the error
                throw new ApplicationException($"Status code: {response.StatusCode}, Error Message: {response.ReasonPhrase}");
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Exception {e}");
            }
        }
    }
}
