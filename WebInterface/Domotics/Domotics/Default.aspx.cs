using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotics
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



        }

        protected void Button1_Click(object sender, EventArgs e)
        {


            DataSetTableAdapters.temperatureTableAdapter temperatures =
                new DataSetTableAdapters.temperatureTableAdapter();
               

           DataSet.temperatureDataTable result =
               temperatures.GetDataByVoid();

           if (result.Count == 0)
           {
               throw new Exception("Bug");
           }else{

               Label1.Text = ((Double) result.Rows[0]["value"]).ToString();
               
          
               
           }
            
            
            Label1.Visible = true;
        }
    }
}