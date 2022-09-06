using System;
using System.Collections;
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
    /// Logika interakcji dla klasy PinTopValue.xaml
    /// </summary>
    public partial class PinTopValue : UserControl
    {
        public int canAddress { get; set; }
        public String canBit { get; set; }

        /// <summary>
        /// konstruktor elementu XAML odpowiadającego za pojedyńczą kolumnę widoku konektora
        /// </summary>
        /// <param name="pinNumber">numer pina do wyświetlenia u dołu kolumny</param>
        /// <param name="canAddress">adres CAN na który ta kontrolka reaguje</param>
        /// <param name="canBit">offsety bitu z którego jest czytana i wyświetlana wartość</param>
        public PinTopValue(int pinNumber, int canAddress, String canBit)
        {
            InitializeComponent();
            PinNumberXAML.Text = pinNumber.ToString();
            this.canAddress = canAddress;
            this.canBit = canBit;
            MainWindow.CANMessageEvent += new EventHandler<CANMessage>(can_event_update_pin);
        }

        /// <summary>
        /// Subskrybent zdarzenia przesyłania wiadomości CAN w MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message">wiadomość CAN</param>
        private void can_event_update_pin(object sender, CANMessage message)
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


        }


        /*private void updatePinValue(object sender, bool pinValue, int pinNumber)
        {
            // każda kolumna subskrybuje ten sam event więc potrzebne jest sprawdzenie czy podany pinNumber znajduje się w kolumnie XAML
            // jeżeli kod przejdzie ten warunek pomyślnie to znaczy, że ta konkretna kolumna musi uaktualnić dane
            if (!(PinNumberXAML.Text.Equals(pinNumber.ToString()) || PinNumberXAML.Text.Equals(pinNumber.ToString())))
            {
                return;
            }

            SolidColorBrush ellipseColor = new SolidColorBrush();
            ellipseColor.Color = (pinValue) ? Color.FromRgb(0, 255, 0) : Color.FromRgb(255, 0, 0);


            // "górny" pin w kolumnie, trzeba uaktualnić kolor górnej kulki
            if (PinNumberXAML.Text.Equals(pinNumber.ToString()))
            {
                // dopisywanie wartoścido PinValueXAML?
            }

         PinNumberXAML.Text = newPinValue.ToString();
            PinNumberXAML.UpdateLayout();

        }*/

    }
}
