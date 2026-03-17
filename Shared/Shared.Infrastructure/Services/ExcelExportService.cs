using OfficeOpenXml;
using OfficeOpenXml.Style;
using Shared.Application.Common.Interfaces;
using Shared.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Services
{
    public class ExcelExportService : IExcelExportService
    {
        private readonly string _templateFolder;

        public ExcelExportService()
        {
            _templateFolder = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "E:/Templates"
                : "/templates";
        }
    }
}
