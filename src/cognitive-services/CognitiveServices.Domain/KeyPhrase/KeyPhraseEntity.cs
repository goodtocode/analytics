using GoodToCode.Shared.Blob.Abstractions;
using System.Text.Json.Serialization;

namespace GoodToCodeAnalytics.CognitiveServices.Domain
{
    public class KeyPhraseEntity : RowEntity, IKeyPhraseEntity
    {
        [JsonInclude]
        public string KeyPhrase { get; private set; }

        public KeyPhraseEntity() { }

        public KeyPhraseEntity(ICellData cell, string keyPhrase) : base(cell)
        {
            KeyPhrase = keyPhrase;
        }
    }
}