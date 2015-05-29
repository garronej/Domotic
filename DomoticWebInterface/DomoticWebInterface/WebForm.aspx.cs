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

            

            if (!IsPostBack)
            {

                this.Init();

            }

        }

        private void Init()
        {

            

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {

            

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

                System.Diagnostics.Debug.WriteLine("Fail getLuminosity()");

                Image1.Visible = false;
            }
             
           


            

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
                Image2.Visible = false;
            }


            


            
            try
            {

                if (BusinessLogic.Manager.isLightAutomaticManagmentOn())
                {
                    Label1.Text = "Off";
                    Label1.ForeColor = System.Drawing.Color.Green;

                }
                else
                {
                    Label1.Text = "On";
                    Label1.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception exception)
            {

                Label1.Text = "_";
                Label1.ForeColor = System.Drawing.Color.Black;
            }
             
            



            try
            {
                Label2.Text = String.Format("{0:0.00}", BusinessLogic.Manager.getTemperature());
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Temperature exception :" + exception.StackTrace ); 

                Label2.Text = "?";

            }

            
            try
            {
                Image3.Visible = true;


                if (BusinessLogic.Manager.isLightOn())
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
                Image3.Visible = false;
            }
             

            
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
                Label4.Text = " ? ";
            }
    

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BusinessLogic.Period period = BusinessLogic.Period.LAST_WEEK;

            switch (DropDownList1.SelectedIndex)
            {
                case( 0 ):
                    period = BusinessLogic.Period.LAST_WEEK;
                    break;
                case( 1 ):
                    period = BusinessLogic.Period.TWELVE_HOUR;
                    break;
                case( 2 ):
                    period = BusinessLogic.Period.TWENTYFOUR_HOUR;
                    break;
                default:
                    break;
            }

            this.LoadTemperatureChart(period);
        }

        protected void LoadTemperatureChart(BusinessLogic.Period period)
        {

            List<Record<Double>> records = Manager.getTemperature(period);






            // set up some data
            List<DateTime> xvals = new List<DateTime>();
            List<Double> yvals = new List<Double>();

            foreach( Record<Double> record in records ){

                xvals.Add(DateTime.Parse(record.date));
                yvals.Add(record.value);

            }



            Chart1.AntiAliasing = AntiAliasingStyles.Graphics;
            Chart1.BackColor = Color.Transparent;



            var chartArea = new ChartArea();
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.LabelStyle.Format = "dd, hh:mm";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 10);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 10);

            chartArea.AxisY.Title = "C°";





            //chartArea.AxisX.Title = "Temperature chart of the last 24h " + ((DateTime)result.Rows[0]["time"]).ToString("dd/MMM");




            chartArea.AxisY.MajorTickMark.Enabled = true;
            chartArea.AxisY.MinorTickMark.Enabled = true;


            Chart1.ChartAreas.Add(chartArea);



            var series = new Series();
            series.Name = "Series1";
            series.ChartType = SeriesChartType.FastLine;
            series.XValueType = ChartValueType.DateTime;
            Chart1.Series.Add(series);

            // bind the datapoints
            Chart1.Series["Series1"].Points.DataBindXY(xvals, yvals);



        }






    }
}