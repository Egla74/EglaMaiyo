using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace EglaMaiyo.Services
{
    public class CurrencyServices
    {
        private readonly HttpClient _httpClient;

        public CurrencyServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal?> ConvertCurrencyAsync(string fromCurrency, string toCurrency, decimal amount)
        {
            var soapEnvelope = $@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                           xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                           xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <ConvertCurrency xmlns=""http://tempuri.org/"">
                  <FromCurrency>{fromCurrency}</FromCurrency>
                  <ToCurrency>{toCurrency}</ToCurrency>
                  <Amount>{amount}</Amount>
                </ConvertCurrency>
              </soap:Body>
            </soap:Envelope>";

            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "\"http://tempuri.org/ConvertCurrency\"");

            var response = await _httpClient.PostAsync("https://currencyconverter.soapapi.com/CurrencyServer.asmx", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var xml = XDocument.Parse(responseContent);

            var result = xml.Descendants()
                            .FirstOrDefault(e => e.Name.LocalName == "ConvertCurrencyResult")?.Value;

            if (decimal.TryParse(result, out var convertedAmount))
                return convertedAmount;

            return null;
        }
    }
}

