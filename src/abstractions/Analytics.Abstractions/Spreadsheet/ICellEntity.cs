using GoodToCode.Shared.Persistence.Abstractions;

namespace GoodToCode.Analytics.Abstractions
{
    public interface ICellEntity : IEntity
    {
        string SheetName { get; }
        string ColumnName { get; }
        int RowIndex { get; }
        string CellValue { get; }
    }
}
