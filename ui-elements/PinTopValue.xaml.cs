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

namespace MiunskeBoardProject.ui_elements
{
    /// <summary>
    /// Logika interakcji dla klasy PinTopValue.xaml
    /// </summary>
    public partial class PinTopValue : UserControl
    {
        public String pinType { get; set; }
        public int canAddress { get; set; }
        public String canBit { get; set; }
        /// <summary>
        /// konstruktor elementu XAML odpowiadającego za pojedyńczą kolumnę widoku konektora
        /// </summary>
        /// <param name="pinNumber">numer pina do wyświetlenia u dołu kolumny</param>
        /// <param name="canAddress">adres CAN na który ta kontrolka reaguje</param>
        /// <param name="pinType">typ pina, 'Value' lub 'Boolean'</param>
        /// <param name="canBit">offsety bitu z którego jest czytana i wyświetlana wartość</param>
        public PinTopValue(int pinNumber, String pinType, int canAddress, String canBit)
        {
            InitializeComponent();
            PinNumberXAML.Text = pinNumber.ToString();
            this.pinType = pinType;
            this.canAddress = canAddress;
            this.canBit = canBit;
        }

        public PinTopValue(int pinNumber)
        {
            InitializeComponent();
            PinNumberXAML.Text = pinNumber.ToString();
        }

        public void test()
        {

        }

        public void updatePinValue(int newPinValue)
        {
            PinNumberXAML.Text = newPinValue.ToString();
            PinNumberXAML.UpdateLayout();
        }

        private void updatePinValue(object sender, bool pinValue, int pinNumber)
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

        }

    }
}
