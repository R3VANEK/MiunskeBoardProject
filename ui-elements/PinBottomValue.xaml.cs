using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MiunskeBoardProject.classes;

namespace MiunskeBoardProject.ui_elements
{
    /// <summary>
    /// Logika interakcji dla klasy PinBottomValue.xaml
    /// </summary>
    public partial class PinBottomValue : UserControl
    {

        public int canAddress { get; set; }
        public String canBit { get; set; }


        /// <summary>
        /// konstruktor elementu XAML odpowiadającego za pojedyńczą kolumnę widoku konektora
        /// </summary>
        /// <param name="pinNumber">numer pina do wyświetlenia u dołu kolumny</param>
        /// <param name="canAddress">adres CAN na który ta kontrolka reaguje</param>
        /// <param name="canBit">offsety bitu z którego jest czytana i wyświetlana wartość</param>
        public PinBottomValue(int pinNumber, int canAddress, String canBit)
        {
            InitializeComponent();
            PinNumberXAML.Text = pinNumber.ToString();
            this.canAddress = canAddress;
            this.canBit = canBit;
            //MainWindow.CANMessageEvent += new EventHandler<CANMessage>(can_event_update_pin);
            MainWindow.Event_RxCanMessage += new EventHandler<CanFoxRxEventArg>(test);
        }



        private void test(object sender, CanFoxRxEventArg e)
        {
            CANMessages msgs = e.CopyOfRxMessages((int)canAddress);

            if (msgs.Count < 1 || msgs.Equals(null))
                return;

            foreach (CANMessage msg in msgs)
            {
                int arrayOffset;
                int newValue;
                if (canBit.Contains('-'))
                {
                    arrayOffset = int.Parse(canBit.Split('-')[0].ToString()) / 8;
                    newValue = msg.data[arrayOffset];
                }
                else
                {
                    arrayOffset = int.Parse(canBit.ToString()) / 8;
                    string binData = Convert.ToString(msg.data[arrayOffset], 2).PadLeft(8, '0');
                    int index = int.Parse(canBit.ToString()) % 8;
                    newValue = Convert.ToInt32(binData.Substring(index, 1));
                }
                PinValueXAML.Dispatcher.Invoke(() =>
                {
                    PinValueXAML.Text = newValue.ToString();
                });
            }
        }


        /// <summary>
        /// Subskrybent zdarzenia przesyłania wiadomości CAN w MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message">wiadomość CAN</param>
        /*private void can_event_update_pin(object sender, CANMessage message)
        {

            if (message.address != canAddress)
                return;

            int arrayOffset;
            int newValue;
            if (canBit.Contains('-'))
            {
                arrayOffset = int.Parse(canBit.Split('-')[0].ToString()) / 8;
                newValue = message.aby_data[arrayOffset];
            }
            else
            {
                arrayOffset = int.Parse(canBit.ToString()) / 8;
                string binData = Convert.ToString(message.aby_data[arrayOffset], 2).PadLeft(8, '0');
                int index = int.Parse(canBit.ToString()) % 8;
                newValue = Convert.ToInt32(binData.Substring(index, 1));

            }

            PinValueXAML.Text = newValue.ToString();
        }*/

    }
}
