using System;
using Microsoft.SPOT;

namespace BoardApplication
{
    class BoardStatus
    {
        private static double LUMINOSITY_THRESHOLD = 1.0;
        private static double TEMPERATURE_THRESHOLD = 18.0;

        private bool _automaticLightManagement=true;
        public bool AutomaticLightMangement {
            get { return _automaticLightManagement; }
            set {
                if (value) {
                    Luminosity = Luminosity;
                }
                _automaticLightManagement = value;
                
                //TODO: notify changes to the server
            }
            
        }

        private bool _automaticHeatherManagement = true;
        public bool AutomaticHeatherMangement
        {
            get { return _automaticHeatherManagement; }
            set
            {
                if (value)
                {
                    Temperature = Temperature;
                }
                _automaticHeatherManagement = value;

                //TODO: notify changes to the server
            }

        }



        private Program board;

        private double _temperature = 0.0;
        public double Temperature {

            get { return _temperature; }
            set {
                _temperature = value;
                if (_temperature < TEMPERATURE_THRESHOLD)
                {
                    if (!_heatherOn) { 
                        board.turnOnHeather();
                    }
                }
                else {
                    if (_heatherOn) {
                        board.turnOffHeather();
                    }
                }

            }
        
        }

        private double _luminosity = 0.0;
        public double Luminosity{
        
            get{ return _luminosity; }
            set{
                _luminosity=value;
                if (_luminosity > LUMINOSITY_THRESHOLD)
                {
                    if (_presence && !_lightOn)
                    {
                        board.turnOnLight();

                    }
                }
                else {
                    if (!_presence && _lightOn)
                    {
                        board.turnOffLight();

                    }
                    
                }
            }
        
        }

        private bool _presence = false;
        public bool Presence {
            get { return _presence; }
            set { _presence = value; }
        }


        private bool _lightOn = false;
        public bool LightOn
        {
            get { return _lightOn; }
            set { _lightOn = value; }
        }


        private bool _heatherOn = false;
        public bool HeatherOn
        {
            get { return _heatherOn; }
            set { _heatherOn = value; }
        }

        public BoardStatus(Program b) {
            this.board = b;
        }





    }
    

}
