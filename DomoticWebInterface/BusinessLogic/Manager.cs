
using System;
using System.Collections.Generic;
using System.Linq;


namespace BusinessLogic
{



    public class Manager
    {

        /* ---------- temperature ----------*/

        public static List<Record<double>> getTemperature(Period period)
        {


            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("period", period.ToString());

            return RESTManager.request<double>(Method.GET, "domotic/temperature", parameters);

        }

        public static double getTemperature()
        {



            List<Record<double>> result = Manager.getTemperature(Period.LAST);

            return result[0].value;
        }

        /* ----- temperature/heater -----*/


        public static bool isHeatingSystemOn()
        {
            List<Record<bool>> result = RESTManager.request<bool>(Method.GET, "domotic/heater/status", null);
            return result[0].value;
        }

        public static bool setHeatingSystem(Action action)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("action", action.ToString());

            List<Record<bool>> result = RESTManager.request<bool>(Method.POST, "domotic/heater/status", parameters);
            return result[0].value;


        }

        /* -- temperature/heater/automatic -- */


        public static bool isHeatingAutomaticManagmentOn()
        {
            List<Record<bool>> result = RESTManager.request<bool>(Method.GET, "domotic/heater/automatic", null);
            return result[0].value;
        }

        public static bool setHeatingAutomaticManagment(Action action)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("action", action.ToString());

            List<Record<bool>> result = RESTManager.request<bool>(Method.POST, "domotic/heater/automatic", parameters);
            return result[0].value;
        }






        /* ---------- luminosity ----------*/

        public static List<Record<double>> getLuminosityRecord()
        {

            return RESTManager.request<double>(Method.GET, "domotic/luminosity", null);

        }


        public static double getLuminosity()
        {

           
            List<Record<double>> result = Manager.getLuminosityRecord();

          
            return result[0].value;

        }

    

        /* ----- luminosity/light -----*/


        public static bool isLightOn()
        {
            List<Record<bool>> result = RESTManager.request<bool>(Method.GET, "domotic/light/status", null);
            return result[0].value;
        }

        public static bool setLight(Action action)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("action", action.ToString());

            List<Record<bool>> result = RESTManager.request<bool>(Method.POST, "domotic/light/status", parameters);
            return result[0].value;


        }

        /* -- luminosity/light/automatic -- */


        public static bool isLightAutomaticManagmentOn()
        {
            List<Record<bool>> result = RESTManager.request<bool>(Method.GET, "domotic/light/automatic", null);
            return result[0].value;
        }

        public static bool setLightAutomaticManagment(Action action)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("action", action.ToString());

            List<Record<bool>> result = RESTManager.request<bool>(Method.POST, "domotic/light/automatic", parameters);
            return result[0].value;
        }


        /* ---------- presence ----------*/

        public static List<Record<bool>> isTherePresence(Period period)
        {


            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("period", period.ToString());

            return RESTManager.request<bool>(Method.GET, "domotic/presence", parameters);

        }

        public static bool isTherePresence()
        {

            List<Record<bool>> result = Manager.isTherePresence(Period.LAST);

            return result[0].value;
        }

        






    }



       

    }