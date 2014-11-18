using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.DbAttributes;

namespace Vancl.TMS.Model
{
    public class BaseDbModel<TKey> : BaseModel
    {
        [Column(IsKey=true)]
        public virtual TKey Id { get; set; }


    }
}
