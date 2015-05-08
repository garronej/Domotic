using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

using Ws.Services;
using Ws.Services.Binding;
using it.polito.Gadgeteer;

namespace BoardApplication
{
    public partial class Program
    {
        private static string hostRunningWCFService = "192.168.1.111:8733";
        private static string boardIpAddress = "192.168.1.202";


        private ThreadSafeQueue queue = new ThreadSafeQueue(100);
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            /*******************************************************************************************
            Modules added in the Program.gadgeteer designer view are used by typing 
            their name followed by a period, e.g.  button.  or  camera.
            
            Many modules generate useful events. Type +=<tab><tab> to add a handler to an event, e.g.:
                button.ButtonPressed +=<tab><tab>
            
            If you want to do something periodically, use a GT.Timer and handle its Tick event, e.g.:
                GT.Timer timer = new GT.Timer(1000); // every second (1000ms)
                timer.Tick +=<tab><tab>
                timer.Start();
            *******************************************************************************************/

            Thread t = new Thread(sendDataToServer);
            t.Start();
            
            
            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");


            ///////// TEST /////////
            TemperatureSensor ts = new TemperatureSensor(6);
            ts.MeasurementComplete += ts_MeasurementComplete;
            ts.RequestMeasurement();


            //////// END TEST /////////
            ethernetJ11D.UseThisNetworkInterface();

            //ethernetJ11D.DebugPrintEnabled = true ;
            //ethernetJ11D.UseDHCP();
            ethernetJ11D.UseStaticIP(boardIpAddress, "255.255.255.0", "192.168.1.1");
            ethernetJ11D.NetworkUp += ethernetJ11D_NetworkUp;
            ethernetJ11D.NetworkDown += ethernetJ11D_NetworkDown;
            
            /*while (true) {
                Mainboard.SetDebugLED(true);
                Thread.Sleep(1000);
                Mainboard.SetDebugLED(false);
                Thread.Sleep(1000);
            }*/
            
        }

        void ts_MeasurementComplete(TemperatureSensor sender, double temperature, double relativeHumidity)
        {
            Debug.Print("my temperature is "+temperature);
        }

        void ethernetJ11D_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            ws.Close();
            Debug.Print("network is down");
        }

        void ethernetJ11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            //Debug.Print("I'm trying to check the address.");
            while (!ethernetJ11D.NetworkInterface.NetworkIsAvailable) {
                Thread.Sleep(1000);
                //Debug.Print("it is " + ethernetJ11D.NetworkInterface.IPAddress);
            }
            //Debug.Print("The ip address is set.");
            //throw new NotImplementedException();
            GT.Timer timer = new GT.Timer(10000);
            timer.Tick += temperature_Getter;
            timer.Start();
#if DEBUG
            Debug.Print("The network is up.");
            Debug.Print("IP Address: " + sender.NetworkSettings.IPAddress);
#endif
            
            ws = new WebService(boardIpAddress, 80, this);
        }


        void sendDataToServer() {
            object item;

            while (queue.pull(out item)) {
                InfoToHost info = (InfoToHost)item;
#if DEBUG
                Debug.Print("Sending to the server " + info);
#endif
                
                
                try
                {
                   
                    string address = "http://" + hostRunningWCFService + "/domotic/insert/"+info.DataType;

                    POSTContent content = Gadgeteer.Networking.POSTContent.CreateTextBasedContent(info.JSONValue);
                    
                    HttpRequest req =
                        HttpHelper.CreateHttpPostRequest(address, content, "text/json");
                    req.AddHeaderField("Content-Type","text/json");
                    req.ResponseReceived += req_ResponseReceived;

                    //Debug.Print("Sending request to " + req.URL);
                    //req.SendRequest();


                }
                catch (System.Exception e)
                {
                    Debug.Print("exception!!!");
                    Debug.Print(e.Message);
                    queue.Close();
                }
            }
        }

        void req_ResponseReceived(HttpRequest sender, HttpResponse response)
        {
            Debug.Print("Response contains : " + response.Text);
        }


        void temperature_Getter(GT.Timer timer)
        {
            //get the temperature
            //Debug.Print("Timer tick");
            double temp = lightSense.ReadVoltage();
            temp = temp * 100 - 273;
            Debug.Print("Temperture is : " + temp);
            InfoToHost info = new InfoToHost();
            info.DataType = "temperature";
            info.Value = temp;
            //queue.push(info);
#if DEBUG
            Debug.Print("Put a temperature");
#endif
            //throw new NotImplementedException();
        }

        public void turnOnLight() {
            Mainboard.SetDebugLED(true);
        }
        public void turnOffLight() {
            Mainboard.SetDebugLED(false);
        }


        private WebService ws { get; set; }
    }
}
