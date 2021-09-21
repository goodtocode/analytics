using GoodToCode.Shared.Persistence.Abstractions;

namespace GoodToCode.Analytics.Matching.Domain
{
    public interface IRowEntity : IEntity
    {
        string SheetName { get; }
        string ColumnName { get; }
        int RowIndex { get; }
        string CellValue { get; }
    }
}
