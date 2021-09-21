﻿using GoodToCode.Analytics.Ingress.Domain;
using GoodToCode.Shared.Blob.Abstractions;
using System;

namespace GoodToCode.Analytics.Ingress.Unit.Tests
{
    public class RowEntityFactory
    {
        public static RowEntity CreateRowEntity()
        {
            var cell = new CellData()
            {
                CellValue = "",
                ColumnIndex = 1,
                ColumnName = "",
                RowIndex = 1,
                SheetIndex = 1,
                SheetName = "",
                WorkbookName = ""
            };

            var row = new RowEntity(Guid.NewGuid().ToString(), cell);
            return row;
        }
    }
}