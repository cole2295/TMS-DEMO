using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Caching;
using org.in2bits.MyXls;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.Util.Extensions
{
    /// <summary>
    /// 可导出为xls
    /// </summary>
    public interface IExportXlsable
    {

    }

    public class ExportXlsAttribute : Attribute
    {
        public string ColumnName { get; set; }
        private int _index = 9999;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public ushort Width { get; set; }
        public ExportXlsAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }
    }

    public static class ListExportXlsExtensions
    {
        private static IDictionary<Type, IDictionary<PropertyInfo, ExportXlsAttribute>> CustomAttributeDic = new Dictionary<Type, IDictionary<PropertyInfo, ExportXlsAttribute>>();
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static byte[] ExportToExcel<T>(this IList<T> list, string sheetName) where T : IExportXlsable
        {
            var type = typeof(T);
            IDictionary<PropertyInfo, ExportXlsAttribute> pe = null;
            if (CustomAttributeDic.Keys.Contains(type))
            {
                pe = CustomAttributeDic[type];
            }
            else
            {
                var properties = type.GetProperties();
                pe = new Dictionary<PropertyInfo, ExportXlsAttribute>();
                foreach (var property in properties)
                {
                    var attr = Attribute.GetCustomAttribute(property, typeof(ExportXlsAttribute));
                    if (attr != null)
                    {
                        pe.Add(property, attr as ExportXlsAttribute);
                    }
                }
                if (pe.Count == 0) throw new Exception(type.ToString() + " has no ExportAttribute property!");
                CustomAttributeDic.Add(type, pe);
            }

            XlsDocument xls = new XlsDocument();
            xls.FileName = sheetName + ".xls";
            int rowIndex = 1;
            int colIndex = 0;
            Worksheet sheet = xls.Workbook.Worksheets.Add(sheetName);//状态栏标题名称
            Cells cells = sheet.Cells;
            var items = pe.OrderBy(x => x.Value.Index);
            foreach (var item in items)
            {
                colIndex++;
                var cell = cells.Add( 1, colIndex, item.Value.ColumnName);
                cell.Font.FontName = "黑体";
                cell.Font.Weight = FontWeight.Bold;
                ColumnInfo ci = new ColumnInfo(xls, sheet);
                ci.ColumnIndexStart = (ushort)(colIndex - 1);
                ci.ColumnIndexEnd = ci.ColumnIndexStart;
                int width = item.Value.Width * 256;//列的宽度计量单位为 1/256 字符宽 
                if (width > ushort.MaxValue) width = ushort.MaxValue;
                if (item.Value.Width > 0) ci.Width = (ushort)width;
                else ci.Width = (ushort)(15 * 256);

                sheet.AddColumnInfo(ci);
            }

            foreach (var obj in list)
            {
                rowIndex++;
                colIndex = 0;
                foreach (var item in items)
                {
                    string value = string.Empty;
                    colIndex++;
                    var v = item.Key.GetValue(obj, null);
                    if (item.Key.PropertyType.IsEnum)
                    {
                        if (v != null)
                        {
                            var enumValue = Enum.Parse(item.Key.PropertyType, Convert.ToInt32(v).ToString());
                            value = EnumHelper.GetDescription(enumValue);
                        }
                    }
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        value = Convert.ToString(v);
                    }
                        XF xf = xls.NewXF();
                    
                    Cell cell = cells.Add(rowIndex, colIndex, value);
                    cell.Format = StandardFormats.Text;
                }
            }
        //    xls.Send();
            return xls.Bytes.ByteArray;
        }
    }
}
