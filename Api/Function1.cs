using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Api
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

[Function("WeatherForecast")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var randomNumber = new Random();
            var temp = 0;

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = temp = randomNumber.Next(-20, 55),
                Summary = GetSummary(temp)
            }).ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(result);
            return response;
        }

        private string GetSummary(int temp)
        {
            var summary = "Mild";
            switch (temp)
            {
                case >= 32:
                    summary = "Hot";
                    break;
                case <= 16 and > 0:
                    summary = "Cold";
                    break;
                case <= 0:
                    summary = "Freezing";
                    break;
            }
            return summary;
        }
    }
}
