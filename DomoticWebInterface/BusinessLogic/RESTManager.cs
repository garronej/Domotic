using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;


using System.Web;

using Newtonsoft.Json;

namespace BusinessLogic
{


    public enum Method
    {
        GET,
        POST
    }

    public enum Period
    {
        LAST,
        TWELVE_HOUR,
        TWENTYFOUR_HOUR,
        LAST_WEEK
    }

    public enum Action
    {
        ON,
        OFF
    }


    class RESTManager
    {

        private static String baseURI = "http://127.0.0.1:8733/";


        public static List<Record<T>> request<T>(Method method, String relativeURI, Dictionary<string, string> parameters)
        {


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RESTManager.baseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = null;

                switch (method)
                {
                    case Method.GET:



                        if (parameters != null)
                        {
                            List<String> list = new List<string>();
                            foreach (var item in parameters)
                            {
                                list.Add(item.Key + "=" + HttpUtility.UrlEncode(item.Value));
                            }
                            relativeURI += "?" + string.Join("&", list);
                                                        

                        }



                        response = client.GetAsync(relativeURI).Result;
                        break;
                    case Method.POST:





                        response = client.PostAsync(relativeURI, new FormUrlEncodedContent(parameters)).Result;
                        break;
                }



                if (response.IsSuccessStatusCode)
                {
                    



                    var tmp = response.Content.ReadAsStringAsync();

                    tmp.Wait();

                    String jsonString = tmp.Result;


                    RootRecord<T> rootRecord;
                    try
                    {
                        rootRecord = JsonConvert.DeserializeObject<RootRecord<T>>(jsonString);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("!!!!Error in parsing !!!!");
                        throw new SystemException();
                    }

                    /* Trace */
                    if (method == Method.GET)
                    {
                        System.Diagnostics.Debug.WriteLine( Environment.NewLine + Environment.NewLine +
                            method.ToString() + " " + RESTManager.baseURI + relativeURI + Environment.NewLine + "Responce : " + jsonString +
                            Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {

                        String paramString = "";
                        if (parameters != null)
                        {
                            List<String> list = new List<string>();
                            foreach (var item in parameters)
                            {
                                list.Add(item.Key + "=" + HttpUtility.UrlEncode(item.Value));
                            }
                            paramString += string.Join("; ", list);
                        }


                        System.Diagnostics.Debug.WriteLine( Environment.NewLine + Environment.NewLine +
                            method.ToString() +
                            " " + RESTManager.baseURI +
                            relativeURI + Environment.NewLine +
                            "parameters : " +
                            paramString +
                            Environment.NewLine + "Responce : " + jsonString +
                            Environment.NewLine + Environment.NewLine);
                    }
                    /* Trace */


                    return rootRecord.record;

                }
                else
                {
                    /* Trace */
                    if (method == Method.GET)
                    {
                        System.Diagnostics.Debug.WriteLine( Environment.NewLine + Environment.NewLine +
                            method.ToString() + " " + RESTManager.baseURI + relativeURI + Environment.NewLine + "Error! " + Environment.NewLine + Environment.NewLine);
                    }
                    else
                    {

                        String paramString = "";
                        if (parameters != null)
                        {
                            List<String> list = new List<string>();
                            foreach (var item in parameters)
                            {
                                list.Add(item.Key + "=" + HttpUtility.UrlEncode(item.Value));
                            }
                            paramString += string.Join("; ", list);
                        }

                        System.Diagnostics.Debug.WriteLine( Environment.NewLine + Environment.NewLine +
                            method.ToString() +
                            " " + RESTManager.baseURI +
                            relativeURI + Environment.NewLine +
                            "parameters : " +
                            paramString +
                            Environment.NewLine + "Error!" +
                            Environment.NewLine + Environment.NewLine);
                    }
                    /* Trace */

                    throw new Exception("RESTManager error");
                }
            }


        }


    }
}
