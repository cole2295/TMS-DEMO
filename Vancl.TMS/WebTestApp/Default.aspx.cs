using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace WebTestApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (CachingService.CachingClient c = new CachingService.CachingClient())
            {
                CachingService.Cache cache = c.Get(this.t_Name.Text);
                if (cache != null)
                    Response.Write(cache.CacheValuek__BackingField.ToString());
            }
        }

        protected void b_submit_Click(object sender, EventArgs e)
        {
            using (CachingService.CachingClient c = new CachingService.CachingClient())
            {
                CachingService.Cache cache = new CachingService.Cache() { CacheNamek__BackingField = this.t_Name.Text, CacheValuek__BackingField = this.t_Value.Text };
                c.Store(cache);
            }
        }
    }
}