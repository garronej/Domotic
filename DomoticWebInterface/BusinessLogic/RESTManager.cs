using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

            System.Diagnostics.Debug.WriteLine("Entering request :"); 

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RESTManager.baseURI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = null;

                switch (method)
                {
                    case Method.GET:

                        System.Diagnostics.Debug.WriteLine("GET");

                        System.Diagnostics.Debug.WriteLine("baseUri : " + RESTManager.baseURI);

                        if (parameters != null)
                        {
                            List<String> list = new List<string>();
                            foreach (var item in parameters)
                            {
                                list.Add(item.Key + "=" + HttpUtility.UrlEncode(item.Value));
                            }
                            relativeURI += "?" + string.Join("&", list);


                            
                            

                        }

                        System.Diagnostics.Debug.WriteLine("RelativeURI : " + relativeURI); 


                        response = client.GetAsync(relativeURI).Result;
                        break;
                    case Method.POST:

                         System.Diagnostics.Debug.WriteLine("POST"); 

                         System.Diagnostics.Debug.WriteLine("baseUri : " + RESTManager.baseURI);
                         System.Diagnostics.Debug.WriteLine("RelativeURI : " + relativeURI);
                         System.Diagnostics.Debug.WriteLine("parameters : " + (new FormUrlEncodedContent(parameters)).ToString());



                        response = client.PostAsync(relativeURI, new FormUrlEncodedContent(parameters)).Result;
                        break;
                }



                if (response.IsSuccessStatusCode)
                {

                    System.Diagnostics.Debug.WriteLine("responce success !"); 


                    var tmp = response.Content.ReadAsStringAsync();

                    tmp.Wait();

                    String jsonString = tmp.Result;

                    RootRecord<T> rootRecord = JsonConvert.DeserializeObject<RootRecord<T>>(jsonString);

                    return rootRecord.record;

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("responce failed !"); 

                    throw new Exception("Error");
                }
            }


        }


    }
}
