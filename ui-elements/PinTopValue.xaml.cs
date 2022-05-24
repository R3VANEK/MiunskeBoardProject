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

        /// <summary>
        /// konstruktor elementu XAML odpowiadającego za pojedyńczą kolumnę widoku konektora
        /// </summary>
        /// <param name="pinNumber">numer pina u dołu kolumny</param>
        public PinTopValue(int pinNumber)
        {
            InitializeComponent();
            PinNumberXAML.Text = pinNumber.ToString();
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
