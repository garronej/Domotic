
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

            return RESTManager.request<double>(Method.GET, "temperature", parameters);

        }

        public static double getTemperature()
        {



            List<Record<double>> result = Manager.getTemperature(Period.LAST);

            return result[0].value;
        }

        /* ----- temperature/heater -----*/


        public static bool isHeatingSystemOn()
        {
            List<Record<bool>> result = RESTManager.request<bool>(Method.GET, "temperature/heater", null);
            return result[0].value;
        }

        public static bool setHeatingSystem(Action action)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("action", action.ToString());

            List<Record<bool>> result = RESTManager.request<bool>(Method.POST, "temperature/heater", parameters);
            return result[0].value;


        }

        /* -- temperature/heater/automatic -- */


        public static bool isHeatingAutomaticManagmentOn()
        {
            List<Record<bool>> result = RESTManager.request<bool>(Method.GET, "temperature/heater/automatic", null);
            return result[0].value;
        }

        public static bool setHeatingAutomaticManagment(Action action)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("action", action.ToString());

            List<Record<bool>> result = RESTManager.request<bool>(Method.POST, "temperature/heater/automatic", parameters);
            return result[0].value;
        }






        /* ---------- luminosity ----------*/

        public static List<Record<double>> getLuminosity(Period period)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("period", period.ToString());

            return RESTManager.request<double>(Method.GET, "luminosity", parameters);

        }

        public static double getLuminosity()
        {

            List<Record<double>> result = Manager.getLuminosity(Period.LAST);

            return result[0].value;
        }

        /* ----- luminosity/light -----*/


        public static bool isLightOn()
        {
            List<Record<bool>> result = RESTManager.request<bool>(Method.GET, "luminosity/light", null);
            return result[0].value;
        }

        public static bool setLight(Action action)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("action", action.ToString());

            List<Record<bool>> result = RESTManager.request<bool>(Method.POST, "luminosity/light", parameters);
            return result[0].value;


        }

        /* -- luminosity/light/automatic -- */


        public static bool isLightAutomaticManagmentOn()
        {
            List<Record<bool>> result = RESTManager.request<bool>(Method.GET, "luminosity/light/automatic", null);
            return result[0].value;
        }

        public static bool setLightAutomaticManagment(Action action)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("action", action.ToString());

            List<Record<bool>> result = RESTManager.request<bool>(Method.POST, "luminosity/light/automatic", parameters);
            return result[0].value;
        }


        /* ---------- presence ----------*/

        public static List<Record<bool>> isTherePresence(Period period)
        {


            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("period", period.ToString());

            return RESTManager.request<bool>(Method.GET, "presence", parameters);

        }

        public static bool isTherePresence()
        {

            List<Record<bool>> result = Manager.isTherePresence(Period.LAST);

            return result[0].value;
        }

        






    }



       

    }