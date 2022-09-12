using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;

namespace MiunskeBoardProject.classes
{


    public class CanFoxEventArg : EventArgs
    {
        public CanFoxEventArg()
            : base()
        {
        }
    }


    public class CanFoxRxEventArg : CanFoxEventArg
    {
        private CANMessages _RxMessages = null;
        public CanFoxRxEventArg(CANMessages RxMessages) : base()
        {
            _RxMessages = RxMessages;
        }
        public CanFoxRxEventArg(CanFoxRxEventArg EventArg) : base()
        {
            _RxMessages = EventArg.CopyOfRxMessages();
        }

        public CANMessages CopyOfRxMessages()
        {
            return new CANMessages(_RxMessages);
        }

        public CANMessages CopyOfRxMessages(int ID)
        {
            int[] IDs = { ID };
            return CopyOfRxMessages(IDs);
        }
        public CANMessages CopyOfRxMessages(int[] IDs)
        {
            CANMessages RxOut = _RxMessages.FindAll(
                delegate (CANMessage Message)
                {
                    if (IDs == null) return false;
                    for (int i = 0; i < IDs.Length; i++)
                    {
                        if (Message.id == IDs[i])
                            return true;
                    }
                    return false;
                }
                );
            return RxOut;
        }
    }




    public class CANMessages : List<CANMessage>
    {
        public CANMessages()
            : base()
        {
        }
        public CANMessages(CANMessages Messages)
            : base(Messages)
        {
        }
        private CANMessages(List<CANMessage> Messages)
            : base(Messages)
        {
        }
        public CANMessages(int Capacity)
            : base(Capacity)
        {
        }
        public CANMessages FindAll(Predicate<CANMessage> match)
        {
            return new CANMessages(base.FindAll(match));
        }
    }

    public class CANMessage
    {
        public struct sCANMessage
        {
            private int _id;
            private byte _len;
            private byte _msg_lost;
            private byte _extended;
            private byte _remote;
            private byte[] _data;
            private uint _tstamp;


            public sCANMessage(int Id, uint Tstamp, byte[] Data, byte Len = 8, byte Msg_lost = 0, byte Extended = 0, byte Remote = 0)
            {
                _id = Id;
                _len = Len;
                _msg_lost = Msg_lost;
                _extended = Extended;
                _remote = Remote;
                _tstamp = Tstamp;
                _data = new byte[8];
                this.data = Data;
            }

            public int id { get { return _id; } set { _id = value; } }
            public byte len { get { return _len; } set { _len = Math.Min(value, (byte)8); } }
            public byte msg_lost { get { return _msg_lost; } set { _msg_lost = value; } }
            public byte extended { get { return _extended; } set { _extended = value; } }
            public byte remote { get { return _remote; } set { _remote = value; } }
            public uint tstamp { get { return _tstamp; } set { _tstamp = value; } }
            public byte Byte1 { get { return _data[0]; } set { _data[0] = value; } }
            public byte Byte2 { get { return _data[1]; } set { _data[1] = value; } }
            public byte Byte3 { get { return _data[2]; } set { _data[2] = value; } }
            public byte Byte4 { get { return _data[3]; } set { _data[3] = value; } }
            public byte Byte5 { get { return _data[4]; } set { _data[4] = value; } }
            public byte Byte6 { get { return _data[5]; } set { _data[5] = value; } }
            public byte Byte7 { get { return _data[6]; } set { _data[6] = value; } }
            public byte Byte8 { get { return _data[7]; } set { _data[7] = value; } }


            public bool GetBit(int bit)
            {
                if ((bit < 0) || (bit > 63))
                    return false;
                byte B = 0;
                if (bit < 8)
                    B = Byte1;
                else if (bit < 16)
                    B = Byte2;
                else if (bit < 24)
                    B = Byte3;
                else if (bit < 32)
                    B = Byte4;
                else if (bit < 40)
                    B = Byte5;
                else if (bit < 48)
                    B = Byte6;
                else if (bit < 56)
                    B = Byte7;
                else
                    B = Byte8;

                int b = bit % 8;

                if (b == 0)
                    return (B & 0x80) == 0x80;
                else if (b == 1)
                    return (B & 0x40) == 0x40;
                else if (b == 2)
                    return (B & 0x20) == 0x20;
                else if (b == 3)
                    return (B & 0x10) == 0x10;
                else if (b == 4)
                    return (B & 0x08) == 0x08;
                else if (b == 5)
                    return (B & 0x04) == 0x04;
                else if (b == 6)
                    return (B & 0x02) == 0x02;
                else
                    return (B & 0x01) == 0x01;

            }

            public byte[] data
            {
                get { return (byte[])_data.Clone(); }
                set
                {
                    if (value != null)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (value.Length > i)
                                _data[i] = value[i];
                            else
                                _data[i] = 0;
                        }
                    }
                    else
                        for (int i = 0; i < 8; i++)
                            _data[i] = 0;

                }
            }

        }
        #region Variables
        int _id;
        byte _len;
        byte _msg_lost;
        byte _extended;
        byte _remote;
        byte[] _data = new byte[8];
        uint _tstamp;

        public const int unmanagedsize = 20;
        #endregion


        public IntPtr CopyDataToPointer()
        {
            IntPtr pCANStruct = Marshal.AllocHGlobal(unmanagedsize);

            Marshal.WriteInt32(pCANStruct, 0, this.id);
            Marshal.WriteByte(pCANStruct, 4, this.len);
            Marshal.WriteByte(pCANStruct, 5, this.msg_lost);
            Marshal.WriteByte(pCANStruct, 6, this.extended);
            Marshal.WriteByte(pCANStruct, 7, this.remote);
            for (int i = 0; i < 8; i++)
            {
                Marshal.WriteByte(pCANStruct, 8 + i, _data[i]);
            }

            return pCANStruct;
        }

        internal CANMessage(IntPtr pCANStruct)
        {
            if (pCANStruct != IntPtr.Zero)
            {
                _id = Marshal.ReadInt32(pCANStruct, 0);
                _len = Marshal.ReadByte(pCANStruct, 4);
                _msg_lost = Marshal.ReadByte(pCANStruct, 5);
                _extended = Marshal.ReadByte(pCANStruct, 6);
                _remote = Marshal.ReadByte(pCANStruct, 7);
                for (int i = 0; i < 8; i++)
                {
                    _data[i] = Marshal.ReadByte(pCANStruct, 8 + i);
                }
                _tstamp = (uint)Marshal.ReadInt32(pCANStruct, 16);
            }
        }
        public CANMessage(sCANMessage Message)
        {
            _id = Message.id;
            _len = Message.len;
            _msg_lost = Message.msg_lost;
            _extended = Message.extended;
            _remote = Message.remote;
            _tstamp = Message.tstamp;
            this.data = Message.data;
        }

        public int id { get { return _id; } }
        public byte len { get { return _len; } }
        public byte msg_lost { get { return _msg_lost; } }
        public byte extended { get { return _extended; } }
        public byte remote { get { return _remote; } }
        public uint tstamp { get { return _tstamp; } }
        public byte Byte1 { get { return _data[0]; } }
        public byte Byte2 { get { return _data[1]; } }
        public byte Byte3 { get { return _data[2]; } }
        public byte Byte4 { get { return _data[3]; } }
        public byte Byte5 { get { return _data[4]; } }
        public byte Byte6 { get { return _data[5]; } }
        public byte Byte7 { get { return _data[6]; } }
        public byte Byte8 { get { return _data[7]; } }
        public bool GetBit(int bit)
        {
            if ((bit < 0) || (bit > 63))
                return false;
            byte B = 0;
            if (bit < 8)
                B = Byte1;
            else if (bit < 16)
                B = Byte2;
            else if (bit < 24)
                B = Byte3;
            else if (bit < 32)
                B = Byte4;
            else if (bit < 40)
                B = Byte5;
            else if (bit < 48)
                B = Byte6;
            else if (bit < 56)
                B = Byte7;
            else
                B = Byte8;

            int b = bit % 8;

            if (b == 0)
                return (B & 0x80) == 0x80;
            else if (b == 1)
                return (B & 0x40) == 0x40;
            else if (b == 2)
                return (B & 0x20) == 0x20;
            else if (b == 3)
                return (B & 0x10) == 0x10;
            else if (b == 4)
                return (B & 0x08) == 0x08;
            else if (b == 5)
                return (B & 0x04) == 0x04;
            else if (b == 6)
                return (B & 0x02) == 0x02;
            else
                return (B & 0x01) == 0x01;

        }


        public byte[] data
        {
            get { return (byte[])_data.Clone(); }
            private set
            {
                if (value != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (value.Length > i)
                            _data[i] = value[i];
                        else
                            _data[i] = 0;
                    }
                }
                else
                    for (int i = 0; i < 8; i++)
                        _data[i] = 0;

            }
        }
    }


    public class CANFox
    {
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
        public enum BITS : byte
        {
            BIT1 = 0x01,
            BIT2 = 0x02,
            BIT3 = 0x04,
            BIT4 = 0x08,
            BIT5 = 0x10,
            BIT6 = 0x20,
            BIT7 = 0x40,
            BIT8 = 0x80,
            nBIT1 = (0xFF) - 0x01,
            nBIT2 = (0xFF) - 0x02,
            nBIT3 = (0xFF) - 0x04,
            nBIT4 = (0xFF) - 0x08,
            nBIT5 = (0xFF) - 0x10,
            nBIT6 = (0xFF) - 0x20,
            nBIT7 = (0xFF) - 0x40,
            nBIT8 = (0xFF) - 0x80,
        }

        public enum Baudrates : uint
        {
            BAUD_1000KBIT = 0,
            BAUD_800KBIT = 1,
            BAUD_500KBIT = 2,
            BAUD_250KBIT = 3,
            BAUD_125KBIT = 4,
            BAUD_100KBIT = 5,
            BAUD_50KBIT = 6,
            BAUD_20KBIT = 7,
            BAUD_10KBIT = 8,
            BAUD_83KBIT = 0x8039C02F,
            BAUD_USERDEFINED = 0x80000000,
            BAUD_SILENTMODE = 0x40000000,
            BAUD_LOWSPEED = 0x20000000,
            BAUD_TXWITHOUTACK = 0x10000000
        }
        public enum CANUpdateInterval : uint
        {
            Intervall_200ms = 200,
            Intervall_250ms = 250,
            Intervall_300ms = 300,
            Intervall_350ms = 350,
            Intervall_400ms = 400,
            Intervall_500ms = 500,
            Intervall_600ms = 600,
            Intervall_750ms = 750,
            Intervall_1000ms = 1000,
        }
        #region Varaibles
        private static IntPtr NULL = (IntPtr)0;

        private static IntPtr _handle = (IntPtr)0;
        private const Int32 _l_netnumber = 105;
        private static Int32 _l_mode = 0;
        private static Int32 _l_echoon = 0;
        private static Int32 _l_txtimeout = 1000;
        private static Int32 _l_rxtimeout = 1000;
        private static Int32 _lastreturn = 0;
        private static Baudrates _l_baud = Baudrates.BAUD_250KBIT;
        //private static Baudrates _l_baud = Baudrates.BAUD_83KBIT ;
        private static int _CANReadIntervall = 500; //in ms
        private static Timer CANRxTimer = new Timer();
        private static bool _isOpen = false;
        private static List<int> _IDs = new List<int>();
        public static CANMessages Messages = new CANMessages();
        public const Int32 MAX_CAN_BUFFER = 4096 * 8;
        private IntPtr pCanBuffer = IntPtr.Zero;
        private Int32 bufferlength = MAX_CAN_BUFFER;
        private FileStream fs = null;
        private TextWriter tw = null;
        private int FSCount = -1;
        private const int FSMaxCount = 6000;
        #endregion

        public delegate void Event_RxCanMessage_Callback(object sender, CanFoxRxEventArg e);


        #region Events and stuff
        public static event EventHandler<CanFoxRxEventArg> Event_RxCanMessage;

        private void FireEventRxCanMessage(CANMessages newMessages)
        {
            EventHandler<CanFoxRxEventArg> event_RxCanMessage = Event_RxCanMessage;
            if (event_RxCanMessage != null)
            {
                event_RxCanMessage(this, new CanFoxRxEventArg(newMessages));
            }
        }
        #endregion

        #region Constructor and Destructor
        public CANFox()
        {
            Init();
        }
        public CANFox(bool open)
        {
            Init();
            if (open)
            {
                openCan();
            }
        }

        ~CANFox()
        {
            this.closeCAN();
            DeInit();
        }

        private void Init()
        {
            pCanBuffer = Marshal.AllocHGlobal(MAX_CAN_BUFFER * 20);
            Messages.Capacity = 2 * MAX_CAN_BUFFER;
        }
        private void DeInit()
        {
            stopRxTimer();
            if (pCanBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pCanBuffer);
            }
            if (tw != null) { try { tw.Close(); } catch { }; }
        }
        #endregion

        public ErrorCode openCan()
        {
            
                IntPtr phandle = Marshal.AllocHGlobal(IntPtr.Size);

                _lastreturn = canOpen(_l_netnumber,
                        _l_mode,
                        _l_echoon,
                        _l_txtimeout,
                        _l_rxtimeout,
                        "myApp".ToCharArray(),
                        "eventRx".ToCharArray(),
                        "eventRy".ToCharArray(),
                        phandle);

                _handle = Marshal.ReadIntPtr(phandle);
                Marshal.FreeHGlobal(phandle);
                if (LastReturn == ErrorCode.NTCAN_SUCCESS)
                {
                    _isOpen = true;
                }
                else
                {
                    _isOpen = false;
                    _handle = NULL;
                    return LastReturn;
                }

                Baudrate = _l_baud;
                EnableAllIds(true);

                startRxTimer();

            
            
            return LastReturn;
        }

        public ErrorCode closeCAN()
        {
            if (_handle != NULL)
            {
                _lastreturn = canClose(_handle);
                _handle = NULL;
            }

            return LastReturn;
        }

        public ErrorCode EnableAllIds(bool enable)
        {
            _lastreturn = canEnableAllIds(_handle, enable);
            return LastReturn;
        }
        private void startRxTimer()
        {

            CANRxTimer.Interval = _CANReadIntervall;
            CANRxTimer.Elapsed += CANRxTimer_Elapsed;
            CANRxTimer.Start();

        }
        private void stopRxTimer()
        {
            CANRxTimer.Stop();
            CANRxTimer.Elapsed -= CANRxTimer_Elapsed;

        }

        public ErrorCode CANTx(CANMessage msg)
        {
            IntPtr pCANStruct = msg.CopyDataToPointer();
            Int32 Tsp = 0;
            canGetDeviceTimestampBase(_l_netnumber, ref Tsp);
            Marshal.WriteInt32(pCANStruct, 16, Tsp);
            Int32 size = 1;// CANMessage.unmanagedsize;


            return (ErrorCode)canSend(_handle, pCANStruct, ref size);

        }

        void CANRxTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bufferlength = MAX_CAN_BUFFER;
            _lastreturn = canReadNoWait(_handle, pCanBuffer, ref bufferlength);
            CANMessages NewMessages = new CANMessages(bufferlength);
            for (int i = 0; i < bufferlength; i++)
            {
                NewMessages.Add(new CANMessage(pCanBuffer + 20 * i));
            }
            Messages.AddRange(NewMessages);


         
            


            FireEventRxCanMessage(NewMessages);
            if (Messages.Count > MAX_CAN_BUFFER)
            {
                Messages.RemoveRange(0, Messages.Count - MAX_CAN_BUFFER);
            }



        }

        public Baudrates Baudrate
        {
            get { return _l_baud; }
            set
            {
                if (_handle != NULL)
                {
                    _lastreturn = canSetBaudrate(_handle, (Int32)value);
                    if (_lastreturn == 0)
                    {
                        _l_baud = value;
                    }
                }
                else
                {
                    _l_baud = value;
                }
            }
        }

        public CANUpdateInterval CANReadIntervall
        {
            set
            {
                _CANReadIntervall = (int)value;
                CANRxTimer.Interval = _CANReadIntervall;
            }
            get { return (CANUpdateInterval)_CANReadIntervall; }
        }
        public ErrorCode LastReturn
        {
            get
            {
                ErrorCode errorCode;
                if (Enum.TryParse<ErrorCode>(_lastreturn.ToString(), out errorCode))
                {
                    return errorCode;
                }
                else return ErrorCode.NTCAN_ERROR_UNDEFINDE;
            }
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

        [DllImport("SIECA132.DLL", CharSet = CharSet.Ansi)]
        protected static extern Int32 canSetBaudrate(IntPtr handle, Int32 l_baud);

        [DllImport("SIECA132.DLL", CharSet = CharSet.Ansi)]
        protected static extern Int32 canEnableAllIds(IntPtr handle, bool Enable);

        [DllImport("SIECA132.DLL", CharSet = CharSet.Ansi)]
        protected static extern Int32 canReadNoWait(IntPtr handle, IntPtr CMSG, ref Int32 lenBuf);

        [DllImport("SIECA132.DLL", CharSet = CharSet.Ansi)]
        protected static extern Int32 canSend(IntPtr handle, IntPtr CMSG, ref Int32 lenBuf);

        [DllImport("SIECA132.DLL", CharSet = CharSet.Ansi)]
        protected static extern Int32 canGetDeviceTimestampBase(Int32 net_num, ref Int32 tsp);


    }




}
