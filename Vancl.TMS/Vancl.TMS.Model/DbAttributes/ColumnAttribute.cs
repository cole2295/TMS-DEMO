using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Vancl.TMS.Model.DbAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public bool IsKey { get; set; }
        public string Name { get; set; }
        public DbType? DbType { get; set; }
    }
}
