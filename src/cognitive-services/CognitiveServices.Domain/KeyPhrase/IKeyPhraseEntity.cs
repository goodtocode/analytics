using GoodToCode.Analytics.Abstractions;

namespace GoodToCode.Analytics.CognitiveServices.Domain
{
    public interface IKeyPhraseEntity : IRowEntity
    {
        string KeyPhrase { get; }
    }
}