using System;
using Microsoft.SPOT;

namespace BoardApplication
{
    
    class InfoToHost
    {
        public string DataType { get; set; }
        public double Value { get; set; }
        public string JSONValue
        {
            get { return "{ \"Value\": " + Value + "}"; }
        }

        public override string ToString()
        {
            return DataType+" = "+Value;
        }

        
    }
}
