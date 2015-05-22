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

        private static String baseUri = "http://192.168.0.2:9000/";


        public static List<Record<T>> request<T>(Method method, String relativeURI, Dictionary<string, string> parameters)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RESTManager.baseUri);
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

                    RootRecord<T> rootRecord = JsonConvert.DeserializeObject<RootRecord<T>>(jsonString);

                    return rootRecord.record;

                }
                else
                {
                    throw new Exception("Error");
                }
            }


        }


    }
}
