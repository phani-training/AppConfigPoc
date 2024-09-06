using Amazon.Runtime;
using Amazon.AppConfigData;
using Amazon.AppConfigData.Model;
using System;
using System.Text.Json;
namespace AWSServerlessApi.Entities
{
    public class AppConfig
    {
        public string ApplicationId { get; set; }
        public string EnvironmentId { get; set; }
        public string ConfigProfileId { get; set; }

        public async Task<Dictionary<string, object>> GetConfigDetails()
        {
            var credentials = new BasicAWSCredentials("", "");

            var config = new AmazonAppConfigDataConfig { RegionEndpoint = Amazon.RegionEndpoint.APSouth1 };
            // Create an AmazonAppConfigDataClient
            var client = new AmazonAppConfigDataClient(credentials, config);

            // Start the configuration session
            var startConfigurationSessionRequest = new StartConfigurationSessionRequest
            {
                ApplicationIdentifier = ApplicationId,
                EnvironmentIdentifier = EnvironmentId,
                ConfigurationProfileIdentifier = ConfigProfileId
            };

            var startConfigurationSessionResponse = await client.StartConfigurationSessionAsync(startConfigurationSessionRequest);

            // Get the configuration
            var getConfigurationRequest = new GetLatestConfigurationRequest
            {
                ConfigurationToken = startConfigurationSessionResponse.InitialConfigurationToken
            };

            var getConfigurationResponse = await client.GetLatestConfigurationAsync(getConfigurationRequest);

            // Read the configuration data (as a stream)
            using (var reader = new StreamReader(getConfigurationResponse.Configuration))
            {
                var configJson = await reader.ReadToEndAsync();

                // Parse the JSON configuration
                //var myConfig = JsonSerializer.Deserialize<MyConfiguration>(configJson);
                //Console.WriteLine(configJson);
                //dynamic jsonObject = JsonSerializer.Deserialize<dynamic>(configJson);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson, options);
                return dictionary!;
            }
        }
    }
}
