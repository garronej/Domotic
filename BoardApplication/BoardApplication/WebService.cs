using System;
using Microsoft.SPOT;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using Gadgeteer.Modules.Polito;


namespace BoardApplication
{
    class WebService
    {
        private string ip;
        private int port;
        private Thread t;
        private Program board;
        private bool closed;
        private EthernetJ11D ethernetJ11D;

        public WebService(string ipAddress, int p, Program program, EthernetJ11D ethernetJ11D)
        {
            this.ip = ipAddress;
            this.port = p;
            this.board = program;
            this.ethernetJ11D = ethernetJ11D;
            closed = false;
            t = new Thread(StartWebService);
            t.Start();
            
            
        }

        

        private void StartWebService()
        {
            while (!this.ethernetJ11D.NetworkInterface.NetworkIsAvailable)
            {
                Thread.Sleep(1000);
            }
            WebServer.StartLocalServer(ip, port);
            //WebServer.DefaultEvent += webEvent_dispatcher;
            WebServer.DefaultEvent.WebEventReceived += webEvent_dispatcher;
            //WebServer.SetupWebEvent("lights").WebEventReceived += lights_Event;
            
            while (!closed) {
                Thread.Sleep(1000);
            }
        }

        public void Close() {
            this.closed = true;
            WebServer.StopLocalServer();
            t.Join();
        }

        void webEvent_dispatcher(string path, WebServer.HttpMethod method, Responder responder)
        {
            
            if (method.Equals(WebServer.HttpMethod.PUT)) {
                string firstURLParam = path.Substring(0, path.IndexOf("/"));
#if DEBUG
                Debug.Print("Called PUT  " + path);
                Debug.Print("URL starts with " + firstURLParam);
#endif

                switch(firstURLParam){
                    case "lights":
#if DEBUG
                    Debug.Print("Called PUT the lights " + path.Substring(path.LastIndexOf("/")));
#endif
                    putLightsManager(path.Substring(path.IndexOf("/")+1));
                    
                    break;
                    case "automatic_light":
                    putAutomaticLightsManager(path.Substring(path.IndexOf("/") + 1));
                    break;
                    case "automatic_heather":
                    putAutomaticHeatherManager(path.Substring(path.IndexOf("/") + 1));
                    break;
                    case "heather":
                    putHeatherManager(path.Substring(path.IndexOf("/") + 1));

                    break;
                    default:
                    lock (board)
                    {
                        responder.Respond(Encoding.UTF8.GetBytes("\"KO\""), "text/json");
                    }
                    return;
                }
                
            }
            lock (board)
            {
                responder.Respond(Encoding.UTF8.GetBytes("\"OK\""), "text/json");
            }
        }

        private void putHeatherManager(string p)
        {
#if DEBUG
            Debug.Print("Called PUT the heather " + p);
#endif
            if (p.Equals("on"))
            {
                board.turnOnHeather();
            }
            else if (p.Equals("off"))
            {
                board.turnOffHeather();
            }
        }

        private void putAutomaticHeatherManager(string p)
        {
#if DEBUG
            Debug.Print("Called PUT the automatic management of lights " + p);
#endif
            if (p.Equals("on"))
            {
                board.getStatus().AutomaticHeatherMangement = true;
            }
            else if (p.Equals("off"))
            {
                board.getStatus().AutomaticHeatherMangement = false;
            }
        }

        private void putAutomaticLightsManager(string p)
        {
#if DEBUG
            Debug.Print("Called PUT the automatic management of lights " + p);
#endif
            if (p.Equals("on"))
            {
                board.getStatus().AutomaticLightMangement=true;
            }
            else if (p.Equals("off"))
            {
                board.getStatus().AutomaticLightMangement = false;
            }
        }

        private void putLightsManager(string p)
        {
#if DEBUG
            Debug.Print("Called PUT the lights " + p);
#endif
            if (p.Equals("on"))
            {
                board.turnOnLight();
            }
            else if (p.Equals("off"))
            {
                board.turnOffLight();
            }
        }

    }
}
