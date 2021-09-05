using GoodToCode.Shared.Blob.Abstractions;

namespace GoodToCode.Analytics.Domain
{
    public class KeyPhraseEntity : RowEntity, IKeyPhraseEntity
    {
        public string KeyPhrase { get; set; }

        public KeyPhraseEntity() { }

        public KeyPhraseEntity(ICellData cell, string keyPhrase) : base(cell)
        {
            KeyPhrase = keyPhrase;
        }
    }
}