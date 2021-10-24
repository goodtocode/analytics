
using GoodToCode.Shared.Persistence.Abstractions;
using System;

namespace GoodToCode.Analytics.Matching.Unit.Tests
{
    public class DataSourceEntity : IEntity
    {
        private string _partitionKey;
        public string PartitionKey { get { return _partitionKey; } set { _partitionKey = value; } }
        public string RowKey { get; set; } = $"{Guid.NewGuid()}";
        public string Address { get; set; }
        public string ContentType { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public string Indexability { get; set; }
        public string IndexabilityStatus { get; set; }
        public string Title1 { get; set; }
        public string H1_1 { get; set; }
        public string H1_2 { get; set; }
        public string H2_1 { get; set; }
        public string H2_2 { get; set; }

        public DataSourceEntity() { }
        public DataSourceEntity(string partition)
        {
            _partitionKey = partition;
        }
    }
}
