﻿using System;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.SocketInterfaces;
using System.Threading;

namespace Gadgeteer.Modules.Polito
{
    /// <summary>
    /// A PresenceSensor module for Microsoft .NET Gadgeteer
    /// </summary>
    public class PresenceSensor : GTM.Module
    {
        private bool presence = false;
        private Thread t;
        private DateTime _lastTime;
        private DateTime lastTime{ 
            get { 
                lock(this) {
                    return _lastTime;
                }
            }
            set {
                lock (this) {
                    _lastTime = value;
                }
            
            }
        
        }

        // Note: A constructor summary is auto-generated by the doc builder.
        /// <summary></summary>
        /// <param name="socketNumber">The mainboard socket that has the module plugged into it.</param>
        public PresenceSensor(int socketNumber)
        {
            GT.Socket socket = GT.Socket.GetSocket(socketNumber, true, this, null);

            socket.EnsureTypeIsSupported(new char[] { 'X', 'Y' }, this);

            // These calls will throw GT.Socket.InvalidSocketException if a pin conflict or error is encountered
            this.input = GTI.InterruptInputFactory.Create(socket, GT.Socket.Pin.Three, GTI.GlitchFilterMode.On, GTI.ResistorMode.PullUp, GTI.InterruptMode.RisingEdge, this);
            //this.input.Interrupt += (this._input_Interrupt);
            this.led = GTI.DigitalOutputFactory.Create(socket, GT.Socket.Pin.Four, false, this);

            LEDMode = LEDModes.Off;
            t = new Thread(monitoringLastTime);
            t.Start();
        }

        private void _input_Interrupt(GTI.InterruptInput input, bool value)
        {
            //PresenceState buttonState = input.Read() ? PresenceState.SomeoneDetected : PresenceState.NobodyDetected;
            /*switch (buttonState)
            {
                case PresenceState.SomeoneDetected:
                    if (LEDMode == LEDModes.OnWhilePressed)
                        TurnLEDOff();
                    else if (LEDMode == LEDModes.OnWhileReleased)
                        TurnLEDOn();
                    else if (LEDMode == LEDModes.ToggleWhenReleased)
                        ToggleLED();
                    break;
                case PresenceState.NobodyDetected:
                    if (LEDMode == LEDModes.OnWhilePressed)
                        TurnLEDOn();
                    else if (LEDMode == LEDModes.OnWhileReleased)
                        TurnLEDOff();
                    else if (LEDMode == LEDModes.ToggleWhenPressed)
                        ToggleLED();
                    break;
            }*/
            /*if (!presence)
            {
                presence = true;
                this.OnPresenceEvent(this, presence);
            }*/
            //lastTime = System.DateTime.Now;
            
        }

        private GTI.InterruptInput input;
        private GTI.DigitalOutput led;



        /// <summary>
        /// Gets a value that indicates whether someone is detected.
        /// </summary>
        public bool IsDetectedPresence
        {
            get
            {
                //return this.input.Read();
                return presence;
            }
        }

        /// <summary>
        /// Turns on the module's LED.
        /// </summary>
        public void TurnLEDOn()
        {
            led.Write(true);
        }

        /// <summary>
        /// Turns off the module's LED.
        /// </summary>
        public void TurnLEDOff()
        {
            led.Write(false);
        }

        /// <summary>
        /// Toggles the module's LED. If the LED is currently on, it is turned off. If it is currently off, it is turned on.
        /// </summary>
        public void ToggleLED()
        {
            if (IsLedOn)
                TurnLEDOff();
            else
                TurnLEDOn();
        }

        /// <summary>
        /// Gets a boolean value that indicates whether the module's LED is currently lit (true = lit, false = off).
        /// </summary>
        public bool IsLedOn
        {
            get
            {
                return led.Read();
            }
        }

        /// <summary>
        /// Enuerates the various modes a LED can be set to.
        /// </summary>
        public enum LEDModes
        {
            /// <summary>
            /// The LED is on regardless of the button state.
            /// </summary>
            On,
            /// <summary>
            /// The LED is off regardless of the button state.
            /// </summary>
            Off,
            /// <summary>
            /// The LED changes state whenever the button is pressed.
            /// </summary>
            ToggleWhenPressed,
            /// <summary>
            /// The LED changes state whenever the button is released.
            /// </summary>
            ToggleWhenReleased,
            /// <summary>
            ///  The LED is on while the button is pressed.
            /// </summary>
            OnWhilePressed,
            /// <summary>
            /// The LED is on except when the button is pressed.
            /// </summary>
            OnWhileReleased
        }

        private LEDModes _ledMode;

        /// <summary>
        /// Gets or sets the LED's current mode of operation.
        /// </summary>
        public LEDModes LEDMode
        {
            get
            {
                return _ledMode;
            }
            set
            {
                _ledMode = value;

                if (_ledMode == LEDModes.On || _ledMode == LEDModes.OnWhilePressed && !IsDetectedPresence || _ledMode == LEDModes.OnWhileReleased && IsDetectedPresence)
                    TurnLEDOn();
                else if (_ledMode == LEDModes.Off || _ledMode == LEDModes.OnWhileReleased && !IsDetectedPresence || _ledMode == LEDModes.OnWhilePressed && IsDetectedPresence)
                    TurnLEDOff();
            }

        }

        /// <summary>
        /// Represents the delegate that is used to handle the <see cref="ButtonReleased"/>
        /// and <see cref="ButtonPressed"/> events.
        /// </summary>
        /// <param name="sender">The <see cref="PresenceSensor"/> object that raised the event.</param>
        /// <param name="state">The state of the Button</param>
        public delegate void PresenceSensorEventHandler(PresenceSensor sender, bool presence);

        /// <summary>
        /// Raised when the state of <see cref="Button"/> is Released.
        /// </summary>
        /// <remarks>
        /// Implement this event handler and the <see cref="ButtonPressed"/> event handler
        /// when you want to provide an action associated with Button activity.
        /// The state of the Button is passed to the <see cref="ButtonEventHandler"/> delegate,
        /// so you can use the same event handler for both Button states.
        /// </remarks>
        public event PresenceSensorEventHandler SomeoneDetected;



        private PresenceSensorEventHandler onSomeoneDetected;

        /// <summary>
        /// Raises the <see cref="SomeoneDetected"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="PresenceSensor"/> that raised the event.</param>
        /// <param name="pres">Whether someone has been detected or none is more in the room.</param>
        protected virtual void OnPresenceEvent(PresenceSensor sender, bool pres)
        {
            if (this.onSomeoneDetected == null)
            {
                this.onSomeoneDetected = new PresenceSensorEventHandler(OnPresenceEvent);
            }
           // Debug.Print("before check and invoke");
            if (Program.CheckAndInvoke(SomeoneDetected, onSomeoneDetected, sender, pres)) {
             //   Debug.Print("in it");
                this.SomeoneDetected(sender, pres);
            }
            

        }

        private void monitoringLastTime() {
            TimeSpan tenSecs = new TimeSpan(0, 0, 10);
            Thread.Sleep(10000);
            this.input.Interrupt += (this._input_Interrupt);
            while (true) {

                Thread.Sleep(100);
                if (input.Read())
                {
                    TurnLEDOn();
                    lastTime = System.DateTime.Now;
                }
                else {
                    TurnLEDOff();
                }
                if (presence)
                {
                    

                    if (System.DateTime.Now.Subtract(tenSecs).CompareTo(lastTime) > 0)
                    {
                        presence = false;
                        Debug.Print("I'm rising no presence event");
                        this.OnPresenceEvent(this, presence);
                    }
                }
                else {
                    if (System.DateTime.Now.Subtract(tenSecs).CompareTo(lastTime) < 0){
                        Debug.Print("I'm rising presence event");
                        presence = true ;
                        this.OnPresenceEvent(this, presence);
                    }
                }
            
            }
            
        }
    }
}