using System;
using System.Threading.Tasks;
using Amazon.AppConfigData;
using Amazon.AppConfigData.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MyLambdaWebAPI
{
    public class Function
    {
        //private static readonly string AppConfigApplicationId = "yjk7vg3";  // Replace with your AppConfig Application Id
        //private static readonly string AppConfigEnvironmentId = "r436jd9";            // Replace with your AppConfig Environment Id
        //private static readonly string AppConfigConfigurationProfileId = "u4sq54q"; // Replace with your Configuration Profile Id

        private static readonly string AppConfigApplicationId = "k34wxau";  // Replace with your AppConfig Application Id
        private static readonly string AppConfigEnvironmentId = "1aplf7k";            // Replace with your AppConfig Environment Id
        private static readonly string AppConfigConfigurationProfileId = "6d46zem"; // Replace with your Configuration Profile Id

        private static readonly HttpClient httpClient = new HttpClient();

        private readonly IAmazonAppConfigData _appConfigDataClient;

        public Function()
        {
            _appConfigDataClient = new AmazonAppConfigDataClient();
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            try
            {
                string appConfigUrl = $"http://localhost:2772/applications/{AppConfigApplicationId}/environments/{AppConfigEnvironmentId}/configurations/{AppConfigConfigurationProfileId}";

                //var startConfigurationSessionRequest = new StartConfigurationSessionRequest
                //{
                //    ApplicationIdentifier = AppConfigApplicationId,
                //    EnvironmentIdentifier = AppConfigEnvironmentId,
                //    ConfigurationProfileIdentifier = AppConfigConfigurationProfileId
                //};

                //var sessionResponse = await _appConfigDataClient.StartConfigurationSessionAsync(startConfigurationSessionRequest);
                //var getConfigurationRequest = new GetLatestConfigurationRequest
                //{
                //    ConfigurationToken = sessionResponse.InitialConfigurationToken
                //};

                //var configResponse = await _appConfigDataClient.GetLatestConfigurationAsync(getConfigurationRequest);

                //var configData = System.Text.Encoding.UTF8.GetString(configResponse.Configuration.ToArray());

                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = 200,
                //    Body = configData
                //};

                var responseMessage = await httpClient.GetAsync(appConfigUrl);
                var configData = await responseMessage.Content.ReadAsStringAsync();
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = $"Configuration: {configData}",
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
                return response;
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Error retrieving configuration: {ex.Message}");
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = $"Error: {ex.Message}"
                };
            }
        }
    }
}
