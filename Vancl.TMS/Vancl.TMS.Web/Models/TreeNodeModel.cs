using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vancl.TMS.Web.Models
{
    public class TreeNodeModel
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public bool open { get; set; }
        public bool @checked { get; set; }
        public bool nocheck { get; set; }
        public byte icon { get; set; }
        public bool halfCheck { get; set; }
        public IList<TreeNodeModel> children { get; set; }
    }
}