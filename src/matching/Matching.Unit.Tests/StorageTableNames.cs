namespace GoodToCode.Analytics.Matching
{
    public struct StorageTableNames
    {
        public static string RuleTable { get; } = "Filter-Rules";
        public static string DataSourceTable { get; } = "Filter-DataSource";
        public static string SequentialResultsTable { get; } = "Filter-Results-Sequential";
        public static string MultipleResultsTable { get; } = "Filter-Results-Multiple";
    }
}
