using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BusinessLogic;

namespace DomoticWebInterface
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                this.init();

            }

        }

        private void init()
        {
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {


            try
            {
                Image1.Visible = true;

                int lum = (int)Math.Floor((BusinessLogic.Manager.getLuminosity() * 16.0) / 100.0);
                Image1.ImageUrl = "~/img/lum" + lum.ToString() + ".gif";
            }
            catch (Exception exception)
            {
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






    }
}