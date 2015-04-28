using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotics
{
    public partial class Temperature : System.Web.UI.Page
    {

        DataSetTableAdapters.temperatureTableAdapter temperatures =
                new DataSetTableAdapters.temperatureTableAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {

            this.refresh();

        }

            


       

        protected void refresh()
        {
            DataSet.temperatureDataTable result =
               temperatures.GetDataByVoid();


            if (result.Count == 0)
            {
                throw new Exception("Bug");
            }
            else
            {

                






                Label1.Text = result.Rows[0]["time"].ToString() + " : " + ((Double)result.Rows[0]["value"]).ToString() + "C°";

                Double s = 0;
                Double min = 9999;
                Double max = 0;
                String minT = null, maxT = null;

                String startT = result.Rows[0]["time"].ToString();
                String endT = result.Rows[result.Count - 1 ]["time"].ToString();

                for (int i = 0; i < result.Count; i++)
                {
                    Double cur = (Double)result.Rows[i]["value"];
                    String curT = result.Rows[i]["time"].ToString();
                    s = s + cur;

                    if (cur <= min) {
                        min = cur;
                        minT = curT;
                    }
                    if (cur >= max)
                    {
                        max = cur;
                        maxT = curT;
                    }
                }

                s = s / (Double)result.Count;



             

                Label2.Text = "From " + startT + " to " + endT + " : " + ((Double)s).ToString() + " C°";

                Label3.Text = maxT + " : " + ((Double)max).ToString() + " C°";

                Label4.Text = minT + " : " + ((Double)min).ToString() + " C°";
            }

        }


        protected void Button1_Click(object sender, EventArgs e)
        {

            this.refresh();
            
        }

    }
}