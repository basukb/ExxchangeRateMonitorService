using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExxchangeRateMonitorService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient _httpClient;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service started at:{time}", DateTimeOffset.Now);
            _httpClient = new HttpClient();
            return base.StartAsync(cancellationToken);
        }
        
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _httpClient.Dispose();
            _logger.LogInformation("Service has been stopped at: {time}", DateTimeOffset.Now);
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _httpClient.GetAsync("https://api.exchangeratesapi.io/latest?base=SGD&symbols=SGD,INR");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var exxchange = JsonConvert.DeserializeObject<ExxchangeRate>(content).Rates;
                    _logger.LogInformation($"ExxchangeRate 1 SGD = {exxchange.INR} INR  at: {DateTimeOffset.Now}");
                }
                else
                {
                    _logger.LogInformation($"An error occured with status code {response.StatusCode}");
                }
               
                await Task.Delay(60000, stoppingToken);
            }
        }
    }
}
