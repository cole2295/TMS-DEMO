using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Vancl.RFD.LMS.PDA.Core.Model;
using LMS.Util;

namespace Vancl.RFD.LMS.PDA.Core
{
    public class PDAPageBase : Page
    {
        public PDAUserModel CurrentUser { get; set; }

        protected override void OnInit(EventArgs e)
        {
            
        }

        //private PDAUserModel GetCurrentUser()
        //{
        //    if (CookieUtil.ExistCookie("LMSPDA"))
        //    { 
                
        //    }
        //}

    }
}
