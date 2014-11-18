using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Common
{
    public class PmsNotice
    {
        private int Id { get; set; }
        public string Title{get;set;}
        public string NoitceContent{get;set;}
        public string Signature{get;set;}
        public int? CreateBy{get;set;}
        public DateTime? CreateTime{get;set;}
        public int? UpdateBy{get;set;}
        public DateTime? UpdateTime { get; set; }
        public bool IsDelete { get; set; }
        public string DistributionCode { get; set; }
        public int SystemId { get; set; }

    }
}
