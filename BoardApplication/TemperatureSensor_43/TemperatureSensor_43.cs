﻿using System;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.SocketInterfaces;
using System.Threading;

namespace Gadgeteer.Modules.Polito
{
    /// <summary>
    /// A TemperatureSensor module for Microsoft .NET Gadgeteer
    /// </summary>
    /// <summary>
    /// A TemperatureSensor module for Microsoft .NET Gadgeteer
    /// </summary>
    public class TemperatureSensor : GTM.Module
    {
        private GTI.DigitalIO _data;
        private GTI.DigitalOutput _sck;

        private bool _measurementRequested = false;
        private bool _continuousMeasurement = false;

        // Note: A constructor summary is auto-generated by the doc builder.
        /// <summary></summary>
        /// <param name="socketNumber">The socket that this module is plugged in to.</param>
        public TemperatureSensor(int socketNumber)
        {
            // This finds the Socket instance from the user-specified socket number.  
            // This will generate user-friendly error messages if the socket is invalid.
            // If there is more than one socket on this module, then instead of "null" for the last parameter, 
            // put text that identifies the socket to the user (e.g. "S" if there is a socket type S)
            Socket socket = Socket.GetSocket(socketNumber, true, this, null);

            _sck = GTI.DigitalOutputFactory.Create(socket, Socket.Pin.Four, false, this);
            _data = GTI.DigitalIOFactory.Create(socket, Socket.Pin.Three, true, GTI.GlitchFilterMode.Off, GTI.ResistorMode.Disabled, this);

            new Thread(TakeMeasurements).Start();
        }


        private void TakeMeasurements()
        {
            double temperatureReading;

            // Wait for SHT10 sensor to start-up and get to sleep mode
            Thread.Sleep(11);

            while (true)
            {
                if (_measurementRequested || _continuousMeasurement)
                {
                    _measurementRequested = false;

                    temperatureReading = 0;

                    startComunication();

                    sendTemperatureRequest();
                    bool[] reading = readTemperatureIntoBoolArray();

                    closeComunication();

                    temperatureReading = BoolArrayToInt(reading);

                    
                    // Throw event
                    OnMeasurementCompleteEvent(this, temperatureReading);

                    // Wait at least 3.6 seconds before next reading to prevent sensor from warming up
                    Thread.Sleep(3600);
                }

                Thread.Sleep(100);
            }
        }

        private void clock(bool a) {
            Thread.Sleep(1);
            _sck.Write(a);
           
        }

        private void startComunication(){
            _data.Write(true); // init to high value
            clock(true);

            _data.Write(false); //start
            clock(false);

            _data.Write(true); //1
            clock(true);
            clock(false);
            _data.Write(false);
        
        }

        private void sendTemperatureRequest() {

            clock(true); // 0
            clock(false);

            clock(true); // 0
            clock(false);

            _data.Write(true); // 1
            clock(true);
            clock(false);
            _data.Write(false);

            clock(true); // A2 : 0
            clock(false);

            clock(true); // A1 : 0
            clock(false);

            clock(true); // A0 : 0
            clock(false);

            _data.Write(true); // R/W 1
            clock(true);
            clock(false);
        
        }


        private void closeComunication() {
            _data.Write(true); //chiusura
            clock(true);
            clock(false);
            _data.Write(false);
            clock(true);
            _data.Write(true);

            
        }

        private bool[] readTemperatureIntoBoolArray() {

            bool[] response = new bool[8];

            while (_data.Read()) ;

            clock(true);

            bool ack = false; //_data.Read();

            clock(false);

            if (ack) ErrorPrint("Communication failure when trying to read temperature.");

            // Read first byte in
            for (short i = 0; i < 8; i++)
            {
                response[i] = _data.Read();
                clock(true);

                clock(false);
            }

            return response;
            
            
        }


        /// <summary>
        /// Begins a sensor measurement and raises the <see cref="MeasurementComplete"/> event when done.
        /// </summary>
        /// <remarks>
        /// To prevent the sensor from overheating, there is approximately a 3.6 second wait after each measurement.
        /// If you call this method repeatedly in a shorter time span, your requests are not queued. 
        /// Use the <see cref="StartContinuousMeasurements"/> method to obtain continuous measurements.
        /// </remarks>
        public void RequestMeasurement()
        {
            _measurementRequested = true;
        }

        /// <summary>
        /// Starts continuous measurements.
        /// </summary>
        /// <remarks>
        /// <para>
        ///  When you call this method, the <see cref="TemperatureSensor"/> sensor takes a measurement, 
        ///  raises the <see cref="MeasurementComplete"/> event when the measurement is complete, and repeats.
        ///  To stop continuous measurements, call the <see cref="StopContinuousMeasurements"/> method.
        /// </para>
        /// <note>
        ///  To prevent the sensor from overheating, there is approximately a 3.6 second wait between measurements.
        /// </note>
        /// </remarks>
        public void StartContinuousMeasurements()
        {
            _continuousMeasurement = true;
        }

        /// <summary>
        /// Stops continuous measurements.
        /// </summary>
        /// <remarks>
        /// When you call the <see cref="StartContinuousMeasurements"/> method, the <see cref="TemperatureSensor"/> sensor takes a measurement, 
        /// raises the <see cref="MeasurementComplete"/> event when the measurement is complete, and repeats.
        /// Call this method to stop continuous measurements.
        /// </remarks>
        public void StopContinuousMeasurements()
        {
            _continuousMeasurement = false;
        }


        /// <summary>
        /// Returns the integer value represented by an array of Boolean values.
        /// </summary>
        /// <param name="boolArray">The array to get the integer value of.</param>
        /// <returns>The integer value represented by <paramref name="boolArray"/></returns>
        /// <remarks>
        /// This method treats <paramref name="boolArray"/> as a binary representation of an integer value represented
        /// in two's complement.
        /// The last index of <paramref name="boolArray"/>
        /// holds the least significant bit, the second to the last index holds the next significant bit, and so on.
        /// </remarks>
        private int BoolArrayToInt(bool[] boolArray)
        {
            int temp = 0;
            if (boolArray[0]) temp -= 1 << (boolArray.Length - 1);

            for (int i = 1; i < boolArray.Length; i++)
            {
                if (boolArray[i]) temp += 1 << (boolArray.Length - 1 - i);
            }

            return temp;
        }


        /// <summary>
        /// Represents the delegate that is used for the <see cref="MeasurementComplete"/>event.
        /// </summary>
        /// <param name="sender">The <see cref="TemperatureSensor"/> object that raised the event.</param>
        /// <param name="temperature">The measured temperature.</param>
        /// <param name="relativeHumidity">The measured humidity.</param>
        public delegate void MeasurementCompleteEventHandler(TemperatureSensor sender, double temperature);

        /// <summary>
        /// Raised when a temperature and humidity measurement is complete.
        /// </summary>
        public event MeasurementCompleteEventHandler MeasurementComplete;

        private MeasurementCompleteEventHandler _OnMeasurementComplete;

        /// <summary>
        /// Raises the <see cref="MeasurementComplete"/>event.
        /// </summary>
        /// <param name="sender">The <see cref="TemperatureSensor"/> object that raised the event.</param>
        /// <param name="temperature">The measured temperature.</param>
        /// <param name="relativeHumidity">The measured humidity.</param>
        protected virtual void OnMeasurementCompleteEvent(TemperatureSensor sender, double temperature)
        {
            if (_OnMeasurementComplete == null) _OnMeasurementComplete = new MeasurementCompleteEventHandler(OnMeasurementCompleteEvent);
            if (Program.CheckAndInvoke(MeasurementComplete, _OnMeasurementComplete, sender, temperature))
            {
                MeasurementComplete(sender, temperature);
            }
        }
    }
}
