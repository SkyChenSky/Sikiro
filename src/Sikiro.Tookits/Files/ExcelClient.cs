using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.Tookits.Files
{
    #region Excel类
    /// <summary>
    /// Excel类
    /// </summary>
    public class ExcelClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ExcelOption _option;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ExcelClient(IHttpClientFactory clientFactory, ExcelOption option, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _option = option;
            _httpContextAccessor = httpContextAccessor;
        }

        private static readonly List<string> FileType = new List<string> { "vnd.ms-excel", "vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

        private async Task<T> Post<T>(string url, object obj) where T : class
        {
            var factory = _clientFactory.CreateClient();
            var response = await factory.PostAsJsonAsync(url, obj);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            return null;
        }

        /// <summary>
        /// 判断是否支持上传类型
        /// </summary>
        /// <param name="suffixStr"></param>
        private static void CheckFileType(string suffixStr)
        {
            var suffix = FileType.FirstOrDefault(a => suffixStr.Contains(a));
            if (suffix.IsNullOrWhiteSpace())
                throw new TookitsFilesException("不支持该文件类型");
        }

        #region 导出
        /// <summary>
        /// 网络中导出 Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="fileName">文件名称（不需要后缀）</param>
        public void HttpExport<T>(IEnumerable<T> source, string fileName = "")
        {
            var wb = CreateExcel(source);

            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss");

            _httpContextAccessor.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            _httpContextAccessor.HttpContext.Response.Headers.Add("Content-Disposition", $"attachment;filename={fileName}.xlsx");
            wb.Write(_httpContextAccessor.HttpContext.Response.Body);
        }

        public async Task<string> HttpExportAsync<T>(IEnumerable<T> source)
        {
            var wb = CreateExcel(source);

            using (var ms = new MemoryStream())
            {
                wb.Write(ms);

                var base64Str = Convert.ToBase64String(ms.GetBuffer());

                var data = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64Str}";

                var sr = await Post<ServiceResult>(_option.Url, new { Data = data });

                if (sr.Success)
                    return (string)sr.Data;

                return null;
            }
        }

        /// <summary>
        /// 导出Excel到本地
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="filepath">文件保存路径</param>
        public void FileExport<T>(IEnumerable<T> source, string filepath)
        {
            var wb = CreateExcel(source);
            using (var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write))
            {
                wb.Write(fs);
            }
        }

        /// <summary>
        /// 创建excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        private XSSFWorkbook CreateExcel<T>(IEnumerable<T> source)
        {
            var wb = new XSSFWorkbook();
            var sh = (XSSFSheet)wb.CreateSheet("Sheet1");

            var props = wb.GetProperties();
            props.CoreProperties.Creator = "葛氏国际物流";
            props.CoreProperties.Created = DateTime.Now;

            var properties = typeof(T).GetProperties().Where(a => a.GetCustomAttribute<ExcelNoMapAttribute>() == null).ToList();

            //构建表头
            var header = sh.CreateRow(0);
            for (var j = 0; j < properties.Count; j++)
            {
                var display = properties[j].GetCustomAttribute<DisplayAttribute>();
                var name = display?.Name ?? properties[j].Name;
                var headCell = header.CreateCell(j);
                headCell.CellStyle = GetHeadStyle(wb);
                headCell.SetCellValue(name);
            }
            var list = source.ToList();

            //填充数据
            for (var i = 0; i < list.Count; i++)
            {
                var r = sh.CreateRow(i + 1);
                for (var j = 0; j < properties.Count; j++)
                {
                    var value = properties[j].GetValue(list[i], null).ToStr();
                    if (properties[j].PropertyType == typeof(DateTime))
                    {
                        var dataTimeCell = r.CreateCell(j);
                        dataTimeCell.CellStyle = DataTimeStyle(wb);
                        dataTimeCell.SetCellValue(value.TryDateTime());
                    }
                    else if (properties[j].PropertyType == typeof(bool))
                    {
                        r.CreateCell(j).SetCellValue(value.TryBool());
                    }
                    else if (properties[j].PropertyType == typeof(int) || properties[j].PropertyType == typeof(decimal) ||
                             properties[j].PropertyType == typeof(float) ||
                             properties[j].PropertyType == typeof(double) || properties[j].PropertyType == typeof(long))
                    {
                        r.CreateCell(j).SetCellValue(value.TryDouble());
                    }
                    else
                    {
                        r.CreateCell(j).SetCellValue(value);
                    }

                    sh.AutoSizeColumn(j);
                }
            }

            return wb;
        }
        #endregion

        #region 导入
        /// <summary>
        /// 本地导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IEnumerable<T> FileImport<T>(string fileName) where T : new()
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open))
            {
                var extName = fileName.Substring(fileName.LastIndexOf('.') + 1).Replace("\"", "");
                return GetDataFromExcel<T>(fileStream, extName == "xlsx");
            }
        }

        /// <summary>
        /// 网络导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public IEnumerable<T> HttpImport<T>(string base64String) where T : new()
        {
            var fileSplit = base64String.Split(',');

            var suffixStr = fileSplit[0];
            var fileBase64Str = fileSplit[1];

            CheckFileType(suffixStr);

            using (var stream = new MemoryStream(Convert.FromBase64String(fileBase64Str)))
            {
                return GetDataFromExcel<T>(stream, suffixStr.Contains("vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            }
        }

        /// <summary>
        /// 网络导入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public IEnumerable<T> HttpImport<T>(IFormFile formFile) where T : new()
        {
            using (var stream = formFile.OpenReadStream())
            {
                var extName = formFile.FileName.Substring(formFile.FileName.LastIndexOf('.') + 1).Replace("\"", "");
                return GetDataFromExcel<T>(stream, extName == "xlsx");
            }
        }

        /// <summary>
        /// 从excel获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelStrem"></param>
        /// <param name="isXlsx">是否xlsx文件</param>
        /// <returns></returns>
        private IEnumerable<T> GetDataFromExcel<T>(Stream excelStrem, bool isXlsx) where T : new()
        {
            IWorkbook wb;
            ISheet sh;
            var list = new List<T>();

            if (isXlsx)
            {
                wb = new XSSFWorkbook(excelStrem);
                if (!(wb.GetSheetAt(0) is XSSFSheet))
                    return list;

                sh = wb.GetSheetAt(0) as XSSFSheet;
            }
            else
            {
                wb = new HSSFWorkbook(excelStrem);

                if (!(wb.GetSheetAt(0) is HSSFSheet))
                    return list;

                sh = wb.GetSheetAt(0) as HSSFSheet;
            }


            var header = sh.GetRow(0);
            var columns = header.Cells.Select(cell => cell.StringCellValue).ToArray();
            var importEntityList = typeof(T).GetProperties().Select(property =>
            {
                var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
                var displayName = displayAttribute?.Name;
                return new ImportEntity { PropertyName = property.Name, DisplayName = displayName };
            }).ToArray();

            var dicColumns = new Dictionary<int, string>();
            for (var i = 0; i < columns.Length; i++)
            {
                var entity = importEntityList.FirstOrDefault(a => a.DisplayName == columns[i] || a.PropertyName == columns[i]);
                if (entity != null)
                    dicColumns.Add(i, entity.PropertyName);
            }

            for (var i = 1; i <= sh.LastRowNum; i++)
            {
                var obj = new T();
                var row = sh.GetRow(i);

                foreach (var key in dicColumns.Keys)
                {
                    var property = typeof(T).GetProperty(dicColumns[key]);
                    var pType = property.PropertyType.FullName;
                    var value = row.GetCell(key).ToStr();
                    if (value.IsNullOrEmpty())
                        continue;

                    switch (pType)
                    {
                        case "System.Int32":
                            property.SetValue(obj, value.TryInt());
                            break;
                        case "System.Int64":
                            property.SetValue(obj, value.TryLong());
                            break;
                        case "System.Double":
                            property.SetValue(obj, value.TryDouble());
                            break;
                        case "System.Decimal":
                            property.SetValue(obj, value.TryDecimal());
                            break;
                        case "System.Boolean":
                            property.SetValue(obj, value.TryBool());
                            break;
                        case "System.Single":
                            property.SetValue(obj, value.TryFloat());
                            break;
                        case "System.DateTime":
                            property.SetValue(obj, value.TryDateTime());
                            break;
                        case "System.String":
                            property.SetValue(obj, value);
                            break;
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 获取时间格式
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        private static ICellStyle DataTimeStyle(IWorkbook wb)
        {
            var cellStyle = wb.CreateCellStyle();
            cellStyle.DataFormat = wb.CreateDataFormat().GetFormat("yyyy-MM-dd HH:mm:ss");
            return cellStyle;
        }

        /// <summary>
        /// 获取表头样式
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        private static ICellStyle GetHeadStyle(IWorkbook wb)
        {
            //居中
            var cellStyle = wb.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.FillForegroundColor = HSSFColor.Grey40Percent.Index;

            return cellStyle;
        }
        #endregion
    }
    #endregion
}
