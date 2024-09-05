# Load AWS SDK assemblies
Add-Type -Path "c:\users\phani\.nuget\packages\awssdk.appconfigdata\3.7.400.13\lib\net45\AWSSDK.AppConfigData.dll"
Add-Type  -Path c:\users\phani\.nuget\packages\awssdk.core\3.7.400.13\lib\net45\AWSSDK.Core.dll

# AWS credentials and configuration
$accessKey = ""
$secretKey = ""
$region = "ap-south-1"

# Create AWS credentials
$credentials = [Amazon.Runtime.BasicAWSCredentials]::new($accessKey, $secretKey)

# Create AWS AppConfigData client config
$clientConfig = [Amazon.AppConfigData.AmazonAppConfigDataConfig]::new()
$clientConfig.RegionEndpoint = [Amazon.RegionEndpoint]::GetBySystemName($region)

# Create AWS AppConfigData client
$client = [Amazon.AppConfigData.AmazonAppConfigDataClient]::new($credentials, $clientConfig)

# Parameters
$applicationId = "drmyok5"
$environmentId = "nq82t4j"
$configurationProfileId = "fass4wi"

# Start the configuration session
$startRequest = [Amazon.AppConfigData.Model.StartConfigurationSessionRequest]::new()
$startRequest.ApplicationIdentifier = $applicationId
$startRequest.EnvironmentIdentifier = $environmentId
$startRequest.ConfigurationProfileIdentifier = $configurationProfileId

# Start configuration session
$startResponse = $client.StartConfigurationSession($startRequest)

# Get the configuration
$getRequest = [Amazon.AppConfigData.Model.GetLatestConfigurationRequest]::new()
$getRequest.ConfigurationToken = $startResponse.InitialConfigurationToken

# Get latest configuration
$getResponse = $client.GetLatestConfiguration($getRequest)

# Read the configuration data (as a stream)
if ($null -ne $getResponse.Configuration) {
    $stream = $getResponse.Configuration
    $reader = [System.IO.StreamReader]::new($stream)
    $configJson = $reader.ReadToEnd()
    $reader.Close()

    # Parse the JSON configuration
    $myConfig = $configJson | ConvertFrom-Json

    # Use the configuration
    Write-Output "The value of 'Setting1' is: $($myConfig.setting1)"
    Write-Output "The value of 'Setting2' is: $($myConfig.setting2)"
    Write-Output "The region: $($myConfig.Mumbai.Region)"
} else {
    Write-Output "No configuration data found."
}
