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

using tempuri.org;
using Ws.Services;
using Ws.Services.Binding;

namespace BoardApplication
{
    public partial class Program
    {
        private bool networkUp = false;
        ThreadSafeQueue queue = new ThreadSafeQueue(100);
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
            GT.Timer timer = new GT.Timer(10000);
            timer.Tick += temperature_Getter;
            timer.Start();

            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");

            ethernetJ11D.UseThisNetworkInterface();

            //ethernetJ11D.DebugPrintEnabled = true ;
            //ethernetJ11D.UseDHCP();
            ethernetJ11D.UseStaticIP("192.168.1.202", "255.255.255.0", "192.168.1.1");
            ethernetJ11D.NetworkUp += ethernetJ11D_NetworkUp;
            ethernetJ11D.NetworkDown += ethernetJ11D_NetworkDown;
            /*while (true) {
                Mainboard.SetDebugLED(true);
                Thread.Sleep(1000);
                Mainboard.SetDebugLED(false);
                Thread.Sleep(1000);
            }*/
            
        }

        void ethernetJ11D_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            networkUp = false;
            Debug.Print("network is down");
        }

        void ethernetJ11D_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            //throw new NotImplementedException();
            Debug.Print("The network is up.");
            Debug.Print("IP Address: " + sender.NetworkSettings.IPAddress);
            networkUp = true;
        }


        void sendDataToServer() {
            object item;
            while (!ethernetJ11D.IsNetworkUp)
            {
                Thread.Sleep(500);
            }
            while (queue.pull(out item)) {
                InfoToHost info = (InfoToHost)item;
                Debug.Print("Sending to the server "+info);
                
                try
                {
                    
                    string hostRunningWCFService = "192.168.1.111:8733";
                    
                    // NOTE: the endpoint needs to match the endpoint of the servicehost
                    string address = "http://" + hostRunningWCFService + "/domotic/insert/"+info.DataType;

                    POSTContent content = Gadgeteer.Networking.POSTContent.CreateTextBasedContent(info.JSONValue);
                    
                    HttpRequest req =
                        HttpHelper.CreateHttpPostRequest(address, content, "text/json");
                    req.AddHeaderField("Content-Type","text/json");
                    req.ResponseReceived += req_ResponseReceived;
                    Debug.Print("Sending request to " + req.URL);
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


        void temperature_Getter(GT.Timer timer)
        {
            //get the temperature
            Debug.Print("Timer tick");
            double temp = 10;
            InfoToHost info = new InfoToHost();
            info.DataType = "temperature";
            info.Value = temp;
            queue.push(info);
            Debug.Print("Put a temperature");
            //throw new NotImplementedException();
        }

    }
}
