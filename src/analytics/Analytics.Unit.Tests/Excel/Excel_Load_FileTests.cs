using GoodToCode.Shared.Blob.Abstractions;
using GoodToCode.Shared.Blob.Excel;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoodToCode.Analytics.Unit.Tests
{
    [TestClass]
    public class Excel_Load_FileTests
    {
        private readonly NpoiBlobReader reader;
        private string SutXlsxFile { get { return @$"{PathFactory.GetProjectSubfolder("Assets")}/AnalysisSimple.xlsx"; } }        

        public ISheetData SutXls { get; private set; }
        public ISheetData SutXlsx { get; private set; }
        public Dictionary<string, StringValues> SutReturn { get; private set; }

        public Excel_Load_FileTests()
        {
            reader = new NpoiBlobReader();
        }

        [TestMethod]
        public void ExcelFile_Read_Xlsx()
        {
            Assert.IsTrue(File.Exists(SutXlsxFile), $"{SutXlsxFile} does not exist. Executing: {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            var sheet = reader.ReadFile(SutXlsxFile).GetSheetAt(0);
            var rows = new List<IRowData>();
            for (int count = sheet.FirstRowNum; count < sheet.LastRowNum; count++)
            {
                var row = sheet.GetRow(count);
                var cells = row.Cells.GetRange(0, row.Cells.Count - 1).Select(c => new CellData() { CellValue = c.StringCellValue, ColumnIndex = c.ColumnIndex, RowIndex = count, SheetName = sheet.SheetName });
                rows.Add(new RowData(count, cells));
            }
            SutXlsx = new SheetData(sheet.SheetName, rows);
            Assert.IsTrue(SutXlsx != null);
            Assert.IsTrue(SutXlsx.Rows.Any(), $"SutXlsx.Rows.Count={SutXlsx.Rows.Count()} > 0");
        }

        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

