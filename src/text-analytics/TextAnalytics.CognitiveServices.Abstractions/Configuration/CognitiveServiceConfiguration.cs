using Microsoft.Extensions.Options;
using System;

namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public class CognitiveServiceConfiguration : ICognitiveServiceConfiguration
    {
        public string KeyCredential { get; set; }

        public string Endpoint { get; set; }

        public CognitiveServiceConfiguration(string keyCredential, string endpoint)
        {
            KeyCredential = keyCredential;
            Endpoint = endpoint;
        }
    }

    public class CognitiveServiceConfigurationValidation : IValidateOptions<CognitiveServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, CognitiveServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.KeyCredential))
                return ValidateOptionsResult.Fail($"{nameof(options.KeyCredential)} configuration parameter is required");

            if (Uri.IsWellFormedUriString(options.Endpoint, UriKind.Absolute))
                return ValidateOptionsResult.Fail($"{nameof(options.Endpoint)} configuration parameter is required");

            return ValidateOptionsResult.Success;
        }
    }
}
