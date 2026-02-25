using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TicketAzure.Core.Dto.CreateEvent;
using TicketAzure.Core.Services;

namespace TicketAzure.infrastructure.Services
{
    public class PaymentExternalService : IPaymentExternalService
    {
        private readonly IConfiguration? _configuration;
        private readonly IMemoryCache? _cache;
        private readonly IStorageService? _storageService;
        private X509Certificate2? certificate21;
        private const string CertificateCacheKey = "Certificate";
        private string ClientId => _configuration?["EfiPayment:client_id"]!;
        private string ClientSecret => _configuration?["EfiPayment:client_secret"]!;

        public async Task<ResponseEfi?> GeneratePixAsync(decimal value)
        {
            var response = await Authenticate();

            var content = await response.Content.ReadAsStringAsync();

            var accessToken = JObject.Parse(content)["access_token"]!.ToString();

            value = 0.02m;

            var valueString = value.ToString("0.00", CultureInfo.InvariantCulture);

            var body = new
            {
                calendario = new
                {
                    expiracao = 900
                },
                valor = new
                {
                    original = valueString
                },
                chave = "123e4567-e89b-12d3-a456-426614174000",
                solicitacaoPagador = "Pagamento de teste"
            };

            HttpClientHandler clientHandler = new();

            if (certificate21 is null)
            {
                throw new Exception("Certificate not found.");
            }

            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

            clientHandler.ClientCertificates.Add(GetCreateCertificate());

            var httpClient = new HttpClient(clientHandler);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://pix.api.efipay.com.br/v2/cob");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var result = await httpClient.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ResponseEfi>(await result.Content.ReadAsStringAsync());
            }

            return null;
        }


        private async Task<HttpResponseMessage> Authenticate()
        {
            string credentials = $"{ClientId}:{ClientSecret}";
            string base64Auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            certificate21 = GetCreateCertificate();

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate21);
            handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            using var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://pix.api.efipay.com.br/")
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "oauth2/token");
            request.Headers.Add("Authorization", $"Basic {base64Auth}");
            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request);

            return response;
        }


        private X509Certificate2 GetCreateCertificate()
        {
            if (_cache.TryGetValue(CertificateCacheKey, out X509Certificate2 certificate))
            {
                return certificate!;
            }

            certificate = GerateCertificate();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                SlidingExpiration = TimeSpan.FromMinutes(10)
            };

            _cache.Set(CertificateCacheKey, certificate, cacheEntryOptions);

            return certificate;
        }


        private X509Certificate2? GerateCertificate()
        {
            var presignedUrl = _storageService.GetSignedUrlAsync("certificado.p12", TimeSpan.FromMinutes(2)).Result;

            certificate21 = LoadCertificateFromBlobStorage(presignedUrl).GetAwaiter().GetResult();
            return certificate21;
        }

        private async Task<X509Certificate2> LoadCertificateFromBlobStorage(string url)
        {
            using HttpClient httpClient = new HttpClient();
            byte[] certData = await httpClient.GetByteArrayAsync(url);

            return new X509Certificate2(certData, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
        }
    }
}
