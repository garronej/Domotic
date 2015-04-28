using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;


namespace Domotics
{
    public partial class Home : System.Web.UI.Page
    {

        DataSetTableAdapters.temperatureTableAdapter temperatures =
                new DataSetTableAdapters.temperatureTableAdapter();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

               

                String script = "alert(\'" + "Je suis javascript" + "\');";

                Page.ClientScript.RegisterStartupScript(
       this.GetType(), "myscript", script, true);


                init();
            }


            
        }






        protected void Timer1_Tick(object sender, EventArgs e)
        {

            DataSet.temperatureDataTable result =
             temperatures.GetDataByVoid();


            Chart1.Series["Series1"].Points.AddXY((DateTime)result.Rows[result.Count - 1]["time"], (Double)result.Rows[result.Count - 1]["value"]);


        }

        protected void init()
        {


            


            DataSet.temperatureDataTable result =
             temperatures.GetDataByVoid();

            // set up some data
            List<DateTime> xvals = new List<DateTime>();
            List<Double> yvals = new List<Double>();

            for (int i = 0; i < result.Count; i++)
            {
                xvals.Add((DateTime)result.Rows[i]["time"]);
                yvals.Add((Double)result.Rows[i]["value"]);
            }

            Chart1.AntiAliasing = AntiAliasingStyles.Graphics;
            Chart1.BackColor = Color.Transparent;
            
            

            var chartArea = new ChartArea();
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.LabelStyle.Format = "hh:mm";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 10);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 10);

            chartArea.AxisY.Title = "C°";
            chartArea.AxisX.Title = "Temperature chart of the last 24h " + ((DateTime)result.Rows[0]["time"]).ToString("dd/MMM");

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