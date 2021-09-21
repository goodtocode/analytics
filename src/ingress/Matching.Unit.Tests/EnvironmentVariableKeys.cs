using System;

namespace GoodToCode.Matching
{
    public static class EnvironmentVariables
    {
        internal static void Validate()
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.AppSettingsConnection)))
                throw new ArgumentNullException(EnvironmentVariableKeys.AppSettingsConnection);
        }
    }

    public struct EnvironmentVariableKeys
    {
        public const string AppSettingsConnection = "AppSettingsConnection";        
        public const string EnvironmentAspNetCore = "ASPNETCORE_ENVIRONMENT";
        public const string EnvironmentAzureFunctions = "AZURE_FUNCTIONS_ENVIRONMENT";
        public const string EnvironmentDotNet = "DOTNET_ENVIRONMENT";
    }

    public struct EnvironmentVariableDefaults
    {
        public const string Environment = "Production";
    }
}
