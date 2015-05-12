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
using Gadgeteer.Modules.Polito;


namespace BoardApplication
{
    public partial class Program
    {
        private static string hostRunningWCFService = "192.168.1.111:8733";
        private static string boardIpAddress = "192.168.1.202";
        private TemperatureSensor temperatureSensor = new TemperatureSensor(6);
        private WebService ws;
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
            
            
            
            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            //Debug.Print("Program Started");

            //starting the timer for the remperature sensor
            temperatureSensor.MeasurementComplete += ts_MeasurementComplete;
            GT.Timer timer = new GT.Timer(60000); // every second (1000ms)
            timer.Tick += temperature_timer_Tick;
            timer.Start();

            //setting up the ethernet interface
            ethernetJ11D.UseThisNetworkInterface();
            ethernetJ11D.UseStaticIP(boardIpAddress, "255.255.255.0", "192.168.1.1");
            ethernetJ11D.NetworkUp += ethernetJ11D_NetworkUp;
            ethernetJ11D.NetworkDown += ethernetJ11D_NetworkDown;

            //starting thread to send data to the server
            Thread t = new Thread(sendDataToServer);
            t.Start();
        }



        void temperature_timer_Tick(GT.Timer timer)
        {
            temperatureSensor.RequestMeasurement();
        }

        void ts_MeasurementComplete(TemperatureSensor sender, double temperature)
        {
            InfoToHost info = new InfoToHost();
            info.DataType = "temperature";
            info.Value = temperature;
            queue.push(info);
#if DEBUG
            Debug.Print("got temperature: " + info.Value);
#endif
            //throw new NotImplementedException();
        }

        void ethernetJ11D_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            if(ws!=null)
                ws.Close();
#if DEBUG
            Debug.Print("network is down");
#endif
        }

        void ethernetJ11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            
#if DEBUG
            Debug.Print("The network is up.");
            Debug.Print("IP Address: " + sender.NetworkSettings.IPAddress);
#endif
            //starting the web service
            ws = new WebService(boardIpAddress, 80, this, this.ethernetJ11D);
        }


        void sendDataToServer() {
            object item;
            while (!this.ethernetJ11D.NetworkInterface.NetworkIsAvailable)
            {
                Thread.Sleep(1000);
            }
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
#if DEBUG
                    Debug.Print("Sending request to " + req.URL);
#endif
                    req.SendRequest();


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


        void light_Getter(GT.Timer timer)
        {
            //get the light
            //Debug.Print("Timer tick");
            //Debug.Print("Light is : " + temp);
            InfoToHost info = new InfoToHost();
            info.DataType = "light";
            info.Value = lightSense.ReadVoltage();
            queue.push(info);
#if DEBUG
            Debug.Print("got light: "+info.Value);
#endif
            //throw new NotImplementedException();
        }

        public void turnOnLight() {
            Mainboard.SetDebugLED(true);
        }
        public void turnOffLight() {
            Mainboard.SetDebugLED(false);
        }


        
    }
}
