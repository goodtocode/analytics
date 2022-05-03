using Microsoft.Extensions.Options;
using System;

namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public class CognitiveServiceOptions : IOptions<ICognitiveServiceConfiguration>
    {
        public ICognitiveServiceConfiguration Value { get; }

        public CognitiveServiceOptions(string keyCredential, string endpoint)
        {
            Value = new CognitiveServiceConfiguration(keyCredential, endpoint);
        }
    }
}
