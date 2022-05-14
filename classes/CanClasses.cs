using System;
using System.Collections.Generic;
using System.Text;

namespace MiunskeBoardProject.classes
{
    public class CANMessage
    {
        public long address { get; set; }
        public long len { get; set; }
        public char[] aby_data { get; set; }
    }
}
