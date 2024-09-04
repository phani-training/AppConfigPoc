using Amazon.AppConfigData;
using Amazon.AppConfigData.Model;
using Amazon.Runtime;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Define your AppConfig parameters
        string applicationId = "drmyok5";
        string environmentId = "nq82t4j";
        string configurationProfileId = "fass4wi";

        var credentials = new BasicAWSCredentials("", "");

        var config = new AmazonAppConfigDataConfig { RegionEndpoint = Amazon.RegionEndpoint.APSouth1 };
        // Create an AmazonAppConfigDataClient
        var client = new AmazonAppConfigDataClient(credentials, config);



        // Start the configuration session
        var startConfigurationSessionRequest = new StartConfigurationSessionRequest
        {
            ApplicationIdentifier = applicationId,
            EnvironmentIdentifier = environmentId,
            ConfigurationProfileIdentifier = configurationProfileId
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

            // Use the configuration
            //Console.WriteLine($"The value of 'Setting1' is: {myConfig.setting1}");
            //Console.WriteLine($"The value of 'Setting2' is: {myConfig.setting2}");
            Console.WriteLine($"The Value of Setting 1 is : {dictionary["setting1"]}");

            ///*Console.WriteLine($"The region: {myConfig.Mumbai.Region}");*/
        }
    }
}

public class MyConfiguration
{
    public string setting1 { get; set; } = string.Empty;
    public string setting2 { get; set; } = string.Empty;
    public RegionDetail Mumbai { get; set; }
}

public class RegionDetail
{
    public string ClientNLB { get; set; }
    public string Region { get; set; }
    public ClientALB ClientALB { get; set; }
}

public class ClientALB
{
    public string Production { get; set; }
    public string Testing { get; set; }
    public string Build { get; set; }
}
