using AddressStandardizer.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AddressStandardizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandardizationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<StandardizationController> _logger;
        private readonly string _dadataApiKey;
        private readonly string _dadataSecretKey;

        public StandardizationController(IHttpClientFactory httpClientFactory, IMapper mapper,
            ILogger<StandardizationController> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _logger = logger;
            _dadataApiKey = configuration["Dadata:ApiKey"];
            _dadataSecretKey = configuration["Dadata:SecretKey"];
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? address)
        {
            try
            {
                if (address == null)
                {
                    _logger.LogDebug("BadRequest: Missing 'address' parameter in the request.");
                    return BadRequest("Missing 'address' parameter in the request.");
                }
                var requestData = new[] { address };
                var json = JsonConvert.SerializeObject(requestData);

                var httpClient = _httpClientFactory.CreateClient("DadataClient");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_dadataApiKey}");
                httpClient.DefaultRequestHeaders.Add("X-Secret", $"{_dadataSecretKey}");

                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("api/v1/clean/address", stringContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var standardizedAddress = _mapper.Map<Rootobject>(content);
                    return Ok(standardizedAddress);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Failed to retrieve standardized address. Response: {ResponseContent}", errorContent);
                    return BadRequest("Failed to retrieve standardized address.");
                }
            }
            catch
            {
                _logger.LogError("Error");
                return StatusCode(500, "An error occurred.");
            }

        }
    }
}
