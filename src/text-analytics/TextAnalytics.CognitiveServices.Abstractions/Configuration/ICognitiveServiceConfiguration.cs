using System;

namespace GoodToCode.Shared.TextAnalytics.Abstractions
{
    public interface ICognitiveServiceConfiguration
    {
        string KeyCredential { get; }
        string Endpoint { get; }
    }
}
