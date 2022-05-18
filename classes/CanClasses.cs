using System;
using System.Collections.Generic;
using System.Text;

namespace MiunskeBoardProject.classes
{
    public class CANMessage
    {
        public int address { get; set; }
        public long len { get; set; }
        public char[] aby_data { get; set; }


        public CANMessage(int address, long len)
        {
            this.address = address;
            this.len = len;
            this.aby_data = new char[len];
        }

    }


    
}
