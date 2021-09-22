
using GoodToCode.Shared.Persistence.Abstractions;
using System;

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    public class EntityA : IEntity
    {
        private string _partitionKey;
        public string PartitionKey { get { return _partitionKey; } set { _partitionKey = value; } }
        public string RowKey { get; set; } = $"{Guid.NewGuid()}";
        public string SomeData { get; set; }

        public EntityA() { }
        public EntityA(string partition)
        {
            _partitionKey = partition;
        }
    }

    public class EntityB : IEntity
    {
        private string _partitionKey;
        public string PartitionKey { get { return _partitionKey; } set { _partitionKey = value; } }
        public string RowKey { get; set; } = $"{Guid.NewGuid()}";
        public string SomeMoreData { get; set; }

        public EntityB() { }
        public EntityB(string partition)
        {
            _partitionKey = partition;
        }
    }
}
