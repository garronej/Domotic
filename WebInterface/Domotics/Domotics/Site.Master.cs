using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotics
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.ContentPlaceHolder.Page.Title.ToString() == "Temperature") m1.CssClass = "onPage";

        }
    }
}