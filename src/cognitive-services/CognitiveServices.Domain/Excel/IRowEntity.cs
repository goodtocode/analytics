using GoodToCode.Shared.Persistence.Abstractions;

namespace GoodToCodeAnalytics.CognitiveServices.Domain
{
    public interface IRowEntity : IEntity
    {
        string SheetName { get; }
        string ColumnName { get; }
        int RowIndex { get; }
        string CellValue { get; }
    }
}
