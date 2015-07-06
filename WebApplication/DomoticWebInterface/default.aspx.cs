using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

using BusinessLogic;

namespace DomoticWebInterface
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script language='javascript'>");                    
            sb.Append(@"permanentScript()");            
            sb.Append(@"</script>");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "permanentScript", sb.ToString(), false);

            if (!IsPostBack)
            {
                this.Initialization();
            }
           
        }

        private void Initialization()
        {
            System.Diagnostics.Debug.WriteLine("===============>Initialisation\n");
            this.UpdateRealTimeValues();
            LoadTemperatureChart(BusinessLogic.Period.TWELVE_HOUR);
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("===============>Tick\n");
            this.UpdateRealTimeValues();             
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("===============>Index changed");


            BusinessLogic.Period period = BusinessLogic.Period.LAST_WEEK;

            switch (DropDownList1.SelectedIndex)
            {
                case (0):
                    period = BusinessLogic.Period.TWELVE_HOUR;
                    break;
                case (1):
                    period = BusinessLogic.Period.TWENTYFOUR_HOUR;
                    break;
                case (2):
                    period = BusinessLogic.Period.LAST_WEEK;
                    break;
                default:
                    break;
            }
            this.LoadTemperatureChart(period);
            LoadingGif2.CssClass = "hidden";
        }


        protected void UpdateRealTimeValues()
        {

            Label3.Text = String.Format("{0:HH:mm:ss}", DateTime.Now);

            //Luminosity value
            try
            {
                Image1.Visible = true;

                double lumPer = (BusinessLogic.Manager.getLuminosity() * 100) / 3.3;

                System.Diagnostics.Debug.WriteLine("lumPer : " + lumPer);


                int lumNum = (int)Math.Floor((lumPer * 16.0) / 100.0);

                System.Diagnostics.Debug.WriteLine("lumNum.ToString() : " + lumNum.ToString());

                Image1.ImageUrl = "~/img/lum" + lumNum.ToString() + ".gif";
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Fail getLuminosity()" + exception.StackTrace);

                Image1.Visible = false;
            }

            //Is light on
            try
            {
                Image2.Visible = true;


                if (BusinessLogic.Manager.isLightOn())
                {
                    Image2.ImageUrl = "~/img/on.gif";
                }
                else
                {

                    Image2.ImageUrl = "~/img/off.gif";
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Exception in isLightOn()" + exception.StackTrace);
                Image2.Visible = false;
            }


            //Light automatic managment
            try
            {
                Image4.Visible = true;

                if (BusinessLogic.Manager.isLightAutomaticManagmentOn())
                {
                    Image4.ImageUrl = "~/img/onAlt.gif";
                }
                else
                {
                    Image4.ImageUrl = "~/img/offAlt.gif";
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Error in isLightAutomaticManagmentOn :" + exception.StackTrace);
                Image4.Visible = false;
            }




            //temperature value
            try
            {
                Label2.Text = String.Format("{0:0.00}", BusinessLogic.Manager.getTemperature());
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Temperature exception :" + exception.StackTrace);

                Label2.Text = "?";

            }

            //Is heater on
            try
            {
                Image3.Visible = true;


                if (BusinessLogic.Manager.isHeatingSystemOn())
                {
                    Image3.ImageUrl = "~/img/on.gif";
                }
                else
                {
                    Image3.ImageUrl = "~/img/off.gif";
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Error in isHeatingSystemOn:" + exception.StackTrace);
                Image3.Visible = false;
            }



            //temperature automatic managment
            try
            {
                Image5.Visible = true;

                if (BusinessLogic.Manager.isHeatingAutomaticManagmentOn())
                {
                    Image5.ImageUrl = "~/img/onAlt.gif";
                }
                else
                {
                    Image5.ImageUrl = "~/img/offAlt.gif";
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Error in isLightAutomaticManagmentOn :" + exception.StackTrace);
                Image4.Visible = false;
            }



            //Presence
            try
            {

                if (Manager.isTherePresence())
                {
                    Label4.Text = "a presence";
                }
                else
                {

                    Label4.Text = "no presence";
                }


            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Error in isTherePresence:" + exception.StackTrace);
                Label4.Text = " ? ";
            }



        }



        protected void LoadTemperatureChart(BusinessLogic.Period period)
        {
            System.Diagnostics.Debug.WriteLine("-------------------In loadTemperatureChart");



            List<Record<Double>> records = Manager.getTemperature(period);

            // set up some data
            List<DateTime> xvals = new List<DateTime>();
            List<Double> yvals = new List<Double>();

            foreach (Record<Double> record in records)
            {

                xvals.Add(DateTime.Parse(record.date));
                yvals.Add(Math.Round(record.value, 1));

            }


            Chart1.AntiAliasing = AntiAliasingStyles.Graphics;
            Chart1.BackColor = Color.Transparent;


            Chart1.ChartAreas["ChartArea1"].BackColor = Color.Transparent;

            switch (period)
            {
                case (BusinessLogic.Period.TWELVE_HOUR):
                    Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{0:HH:mm}";
                    break;
                case (BusinessLogic.Period.TWENTYFOUR_HOUR):
                    Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{0:HH:mm}";
                    break;
                case (BusinessLogic.Period.LAST_WEEK):
                    Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "{0:dd/MM}";
                    break;
                default:
                    break;
            }

            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("Consolas", 10);
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.ForeColor = Color.White;
            Chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new Font("Consolas", 10);
            Chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.ForeColor = Color.White;
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "C°";
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorTickMark.Enabled = true;
            Chart1.ChartAreas["ChartArea1"].AxisY.MinorTickMark.Enabled = true;

            //chartArea.AxisX.Title = "Temperature chart of the last 24h " + ((DateTime)result.Rows[0]["time"]).ToString("dd/MMM");
            //Chart1.ChartAreas.Add(chartArea);

            //var series = new Series();
            //series.Name = "Series1";
            Chart1.Series["Series1"].ChartType = SeriesChartType.Line;
            Chart1.Series["Series1"].XValueType = ChartValueType.DateTime;
            Chart1.Series["Series1"].Color = Color.Green;
            Chart1.Series["Series1"].BorderWidth = 5;

            //Chart1.Series["Series1"]["PointWidth"] = "0.6";
            Chart1.Series["Series1"].IsValueShownAsLabel = true;

            //Chart1.Series.Add(series);

            // bind the datapoints
            Chart1.Series["Series1"].Points.DataBindXY(xvals, yvals);





        }




        protected void Image2_Click(object sender, ImageClickEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("-------------------Click on light interuptor\n");

            try
            {
                Image2.Visible = true;
                if (Image2.ImageUrl.Equals("~/img/on.gif"))
                {
                    if (BusinessLogic.Manager.setLight(BusinessLogic.Action.OFF))
                    {
                        Image2.ImageUrl = "~/img/off.gif";
                    }
                }
                else
                {
                    if (BusinessLogic.Manager.setLight(BusinessLogic.Action.ON))
                    {
                        Image2.ImageUrl = "~/img/on.gif";
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Changing light state exception :" + exception.StackTrace);
                Image2.Visible = false;
            }
            LoadingGif.CssClass = "hidden";
        }

        protected void Image3_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Image3.Visible = true;
                if (Image3.ImageUrl.Equals("~/img/on.gif"))
                {
                    if (BusinessLogic.Manager.setHeatingSystem(BusinessLogic.Action.OFF))
                    {
                        Image3.ImageUrl = "~/img/off.gif";
                    }
                }
                else
                {
                    if (BusinessLogic.Manager.setHeatingSystem(BusinessLogic.Action.ON))
                    {
                        Image3.ImageUrl = "~/img/on.gif";
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Changing heating system state exception :" + exception.StackTrace);
                Image3.Visible = false;
            }
            LoadingGif.CssClass = "hidden";

        }

        protected void Image4_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                Image4.Visible = true;
                if (Image4.ImageUrl.Equals("~/img/onAlt.gif"))
                {
                    if (BusinessLogic.Manager.setLightAutomaticManagment(BusinessLogic.Action.OFF))
                    {
                        Image4.ImageUrl = "~/img/offAlt.gif";
                    }
                }
                else
                {
                    if (BusinessLogic.Manager.setLightAutomaticManagment(BusinessLogic.Action.ON))
                    {
                        Image4.ImageUrl = "~/img/onAlt.gif";
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Error changing light automatic managment state :" + exception.StackTrace);
                Image4.Visible = false;
            }
            LoadingGif.CssClass = "hidden";

        }

        protected void Image5_Click(object sender, ImageClickEventArgs e)
        {

            try
            {
                Image5.Visible = true;
                if (Image5.ImageUrl.Equals("~/img/onAlt.gif"))
                {
                    if (BusinessLogic.Manager.setHeatingAutomaticManagment(BusinessLogic.Action.OFF))
                    {
                        Image5.ImageUrl = "~/img/offAlt.gif";
                    }
                }
                else
                {
                    if (BusinessLogic.Manager.setHeatingAutomaticManagment(BusinessLogic.Action.ON))
                    {
                        Image5.ImageUrl = "~/img/onAlt.gif";
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Error changing heating system automatic managment state :" + exception.StackTrace);
                Image5.Visible = false;
            }
            LoadingGif.CssClass = "hidden";

        }


    }
}