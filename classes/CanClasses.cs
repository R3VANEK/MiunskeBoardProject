using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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


    public class CANFox
    {

        private static Int32 _l_netnumber = 105;
        private static Int32 _l_mode = 0;
        private static Int32 _l_echoon = 0;
        private static Int32 _l_txtimeout = 1000;
        private static Int32 _l_rxtimeout = 1000;
        private static IntPtr phandle = Marshal.AllocHGlobal(IntPtr.Size);
        private static Int32 return_status = 0;
        private static IntPtr _handle = (IntPtr)0;

        public enum ErrorCode
        {

            NTCAN_SUCCESS = 0,
            NTCAN_RX_TIMEOUT = -1,
            NTCAN_TX_TIMEOUT = -2,
            NTCAN_CONTR_BUSOFF = -3,
            NTCAN_NO_ID_ENABLED = -4,
            NTCAN_ID_ALREADY_ENABLED = -5,
            NTCAN_ID_NOT_ENABLED = -6,
            NTCAN_INVALID_PARAMETER = -7,
            NTCAN_INVALID_HANDLE = -8,
            NTCAN_TOO_MANY_HANDLES = -9,
            NTCAN_INIT_ERROR = -10,
            NTCAN_RESET_ERROR = -11,
            NTCAN_DRIVER_ERROR = -12,
            NTCAN_DLL_ALREADY_INIT = -13,
            NTCAN_CHANNEL_NOT_INITIALIZED = -14,
            NTCAN_TX_ERROR = -15,
            NTCAN_NO_SHAREDMEMORY = -16,
            NTCAN_HARDWARE_NOT_FOUND = -17,
            NTCAN_INVALID_NETNUMBER = -18,
            NTCAN_TOO_MANY_J2534_RANGES = -19,
            NTCAN_TOO_MANY_J2534_2_FILTERS = -20,
            NTCAN_DRIVER_NOT_INSTALLED = -21,
            NTCAN_NO_OWNER_RIGHTS = -22,
            NTCAN_FIRMWARE_TOO_OLD = -23,
            NTCAN_FIRMWARE_UNSUPPORTED = -24,
            NTCAN_ERROR_UNDEFINDE = int.MinValue,
        }

        public CANFox()
        {
          
        }

        ~CANFox()
        {
            canClose();
        }


        public ErrorCode canOpen()
        {
            return_status = canOpen(_l_netnumber, _l_mode, _l_echoon, _l_txtimeout, _l_rxtimeout, "MiunskeBoarDProject".ToCharArray(), "eventRx".ToCharArray(),
                       "eventRy".ToCharArray(),
                       phandle);


            _handle = Marshal.ReadIntPtr(phandle);
            Marshal.FreeHGlobal(phandle);

            Baudrate = _l_baud;
            EnableAllIds(true);

            startRxTimer();
        }






        [DllImport("SIECA132.DLL", CharSet = CharSet.Ansi)]
        protected static extern Int32 canOpen(Int32 l_netnumber,
                                               Int32 l_mode,
                                               Int32 l_echoon,
                                               Int32 l_txtimeout,
                                               Int32 l_rxtimeout,
                                               char[] pc_ApplicationName,
                                               char[] pc_ReceiverEvent,
                                               char[] pc_ErrorEvent,
                                               IntPtr phandle);

        [DllImport("SIECA132.DLL", CharSet = CharSet.Ansi)]
        protected static extern Int32 canClose(IntPtr handle);
    }



    
}
