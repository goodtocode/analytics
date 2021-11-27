namespace GoodToCode.Analytics.Matching
{
    public struct StorageTableNames
    {
        public static string RuleMultipleTable { get; } = "FilterRulesMultiple";
        public static string RuleSequentialTable { get; } = "FilterRulesSequential";
        public static string DataSourceTable { get; } = "FilterDataSource";
        public static string ResultsSequentialTable { get; } = "FilterResultsSequential";
        public static string ResultsMultipleTable { get; } = "FilterResultsMultiple";
    }
}
