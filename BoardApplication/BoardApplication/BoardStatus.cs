using System;
using Microsoft.SPOT;

namespace BoardApplication
{
    public class BoardStatus
    {
        private static double LUMINOSITY_THRESHOLD = 1.0;
        private static double TEMPERATURE_THRESHOLD = 18.0;

        private bool _automaticLightManagement=true;
        public bool AutomaticLightMangement {
            get { return _automaticLightManagement; }
            set {
                _automaticLightManagement = value;
                String text = "";
                if (_automaticLightManagement)
                {
                    text = "Manual";
                }
                else
                {
                    text = "Auto";
                }
                board.Auto_light.Text = text;
                board.Auto_light.Invalidate();
                if (value) {
                    Luminosity = Luminosity;
                }

                board.notifyAutomaticLight(_automaticLightManagement);
            }
            
        }

        private bool _automaticHeatherManagement = true;
        public bool AutomaticHeatherMangement
        {
            get { return _automaticHeatherManagement; }
            set
            {
                _automaticHeatherManagement = value;
                String text = "";
                if (_automaticHeatherManagement)
                {
                    text = "Manual";
                }
                else
                {
                    text = "Auto";
                }
                board.Auto_heating.Text = text;
                board.Auto_heating.Invalidate();
                if (value)
                {
                    Temperature = Temperature;
                }

                board.notifyAutomaticHeather(_automaticHeatherManagement);
            }

        }



        private Program board;

        private double _temperature = 0.0;
        public double Temperature {

            get { return _temperature; }
            set {
                _temperature = value;
                if (board.temperatureText != null)
                {
                    board.temperatureText.Text = _temperature.ToString();
                    board.temperatureText.Invalidate();
                }
                if (AutomaticHeatherMangement)
                {
                    if (_temperature < TEMPERATURE_THRESHOLD)
                    {
                        if (!_heatherOn)
                        {
                            board.turnOnHeather();
                        }
                    }
                    else
                    {
                        if (_heatherOn)
                        {
                            board.turnOffHeather();
                        }
                    }
                }
            }
        
        }

        private double _luminosity = 0.0;
        public double Luminosity{
        
            get{ return _luminosity; }
            set{
                _luminosity=value;
                if (board.luminosityText != null) {
                    board.luminosityText.Text = (_luminosity < 1.0) ? "DARK" : (_luminosity < 2.0)?"MEDIUM":"HIGH";
                    board.luminosityText.Invalidate();
                }
                if (AutomaticLightMangement)
                {
                    if (_luminosity < LUMINOSITY_THRESHOLD)
                    {
                        if (_presence && !_lightOn) // sotituire true con _presence
                        {
                            board.turnOnLight();

                        }
                    }
                    else
                    {
                        if (_lightOn)
                        {
                            board.turnOffLight();

                        }

                    }
                }
            }
        
        }

        private bool _presence = false;
        public bool Presence {
            get { return _presence; }
            set { 
                _presence = value;
                if (board.presenceText != null)
                {
                    board.presenceText.Text = _presence?"YES":"NO";
                    board.presenceText.Invalidate();
                }
                if (AutomaticLightMangement)
                {
                    if (_luminosity < LUMINOSITY_THRESHOLD)
                    {
                        if (_presence && !_lightOn)
                        {
                            board.turnOnLight();

                        }
                    }
                    else
                    {
                        if (_lightOn)
                        {
                            board.turnOffLight();

                        }

                    }
                }
                
            }
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
