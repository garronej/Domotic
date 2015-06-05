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
using GHI.Glide;
using GHI.Glide.UI;


namespace BoardApplication
{
    public partial class Program
    {
        private static string hostRunningWCFService = "192.168.1.111:8733";
        private static string boardIpAddress = "192.168.1.202";
        private TemperatureSensor temperatureSensor = new TemperatureSensor(6);
        private PresenceSensor presenceSensor = new PresenceSensor(5);
        private WebService ws;
        private BoardStatus status;
        //private DisplayController display;

        public BoardStatus getStatus() {
            return status;
        }
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
            GHI.Glide.Display.Window Main_menu = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.MAIN_MENU));
            //Debug.Print(Resources.GetString(Resources.StringResources.MAIN_MENU));
            Glide.MainWindow = Main_menu;

            GlideTouch.Initialize();
            setupCallbackManiMenu(Main_menu);
            //display = new DisplayController(this);
            status = new BoardStatus(this);
            presenceSensor.SomeoneDetected += presenceSensor_SomeoneDetected;
            
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


            GT.Timer timer1 = new GT.Timer(60000); // every second (1000ms)
            timer1.Tick += luminosity_Getter;
            timer1.Start();

            //starting thread to send data to the server
            Thread t = new Thread(sendDataToServer);
            t.Start();
        }

        void presenceSensor_SomeoneDetected(PresenceSensor sender, bool presence)
        {
            InfoToHost info = new InfoToHost();
            info.DataType = "presence";
            info.Value = presence?1.0:0.0;
            queue.push(info);
            status.Presence = presence;
#if DEBUG
            Debug.Print("Presence : " + (presence ? "yes" : "no"));
#endif      

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
            status.Temperature = temperature;
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


        void luminosity_Getter(GT.Timer timer)
        {
            //get the light
            //Debug.Print("Timer tick");
            //Debug.Print("Light is : " + temp);
            InfoToHost info = new InfoToHost();
            info.DataType = "luminosity";
            info.Value = lightSense.ReadVoltage();
            queue.push(info);
            status.Luminosity = info.Value;
            

#if DEBUG
            Debug.Print("got light: "+info.Value);
#endif
            //throw new NotImplementedException();
        }

        public void turnOnLight() {
            InfoToHost info = new InfoToHost();
            info.DataType = "light";
            info.Value = 1.0;
            queue.push(info);
            //Mainboard.SetDebugLED(true);
            relayX1.TurnOn();
            status.LightOn = true;
        }
        public void turnOffLight() {
            InfoToHost info = new InfoToHost();
            info.DataType = "light";
            info.Value = 0.0;
            queue.push(info);
            //Mainboard.SetDebugLED(false);
            relayX1.TurnOff();
            status.LightOn = false;
        }

        public void turnOffHeather()
        {
            InfoToHost info = new InfoToHost();
            info.DataType = "heather";
            info.Value = 0.0;
            queue.push(info);
            //Mainboard.SetDebugLED(false);
            multicolorLED.TurnOff();
            status.HeatherOn = false;
        }

        public void turnOnHeather()
        {
            InfoToHost info = new InfoToHost();
            info.DataType = "heather";
            info.Value = 1.0;
            queue.push(info);
            //Mainboard.SetDebugLED(false);
            multicolorLED.TurnRed();
            status.HeatherOn=true;
        }

        public void notifyAutomaticHeather(bool auto) {
            InfoToHost info = new InfoToHost();
            info.DataType = "automatic_heather";
            info.Value = auto?1.0:0.0;
            queue.push(info);
        
        
        }

        public void notifyAutomaticLight(bool auto)
        {
            InfoToHost info = new InfoToHost();
            info.DataType = "automatic_light";
            info.Value = auto ? 1.0 : 0.0;
            queue.push(info);


        }


        public void startMotorUp() {
            motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1,0.5);
        }


        internal void stopMotor()
        {
            motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.2);
        }

        internal void startMotorDown()
        {
            motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.5);
        }

        private void setupCallbackManiMenu(GHI.Glide.Display.Window Main_menu)
        {
            Button Set_heating = (Button)Main_menu.GetChildByName("Set_heating");
            Set_heating.TapEvent += TempTap;

            Button Set_light = (Button)Main_menu.GetChildByName("Set_light");
            Set_light.TapEvent += LumTap;

            Button Up_motor = (Button)Main_menu.GetChildByName("Up_motor");
            Button Down_motor = (Button)Main_menu.GetChildByName("Down_motor");

            Up_motor.PressEvent += Up_motor_PressEvent;
            Up_motor.ReleaseEvent += Up_motor_ReleaseEvent;

            Down_motor.PressEvent += Down_motor_PressEvent;
            Down_motor.ReleaseEvent += Up_motor_ReleaseEvent;
        }

        private void Down_motor_PressEvent(object sender)
        {
            this.startMotorDown();
        }

        private void Up_motor_ReleaseEvent(object sender)
        {
            this.stopMotor();
        }

        void Up_motor_PressEvent(object sender)
        {
            this.startMotorUp();
        }

        private void MenuTap(object sender)
        {
            GHI.Glide.Display.Window Main_menu = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.MAIN_MENU));
            Glide.MainWindow = Main_menu;

            this.setupCallbackManiMenu(Main_menu);
        }

        private void TempTap(object sender)
        {

            GHI.Glide.Display.Window SUB_TEMPERATURE_MENU = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.SUB_TEMPERATURE_MENU));
            Glide.MainWindow = SUB_TEMPERATURE_MENU;
            Button Main_temperature = (Button)SUB_TEMPERATURE_MENU.GetChildByName("Main_temperature");
            Main_temperature.TapEvent += MenuTap;
        }

        private void LumTap(object sender)
        {
            GHI.Glide.Display.Window SUB_LUMINOSITY_MENU = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.SUB_LUMINOSITY_MENU));
            Glide.MainWindow = SUB_LUMINOSITY_MENU;
            Button Main_light = (Button)SUB_LUMINOSITY_MENU.GetChildByName("Main_light");
            Main_light.TapEvent += MenuTap;
        }
    }
}
