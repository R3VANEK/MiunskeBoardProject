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
    /// Logika interakcji dla klasy ConnectorColumn.xaml
    /// </summary>
    public partial class ConnectorColumn : UserControl
    {
        /// <summary>
        /// konstruktor elementu XAML odpowiadającego za pojedyńczą kolumnę widoku konektora
        /// </summary>
        /// <param name="number1">numer pina u góry kolumny</param>
        /// <param name="number2">numer pina na dolu kolumny</param>
        public ConnectorColumn(int number1, int number2)
        {
            InitializeComponent();
            BoxNumber1.Text = number1.ToString();
            BoxNumber2.Text = number2.ToString();
        }



        //potrzebuje wiedziec wartość boolean
        // potrzebuje wiedzieć wartość numer dokładny pina


        private void updatePinValue(object sender, bool pinValue, int pinNumber)
        {
            // każda kolumna subskrybuje ten sam event więc potrzebne jest sprawdzenie czy podany pinNumber znajduje się w kolumnie XAML
            // jeżeli kod przejdzie ten warunek pomyślnie to znaczy, że ta konkretna kolumna musi uaktualnić dane
            if(!(BoxNumber1.Text.Equals(pinNumber.ToString()) || BoxNumber2.Text.Equals(pinNumber.ToString())))
            {
                return;
            }

            SolidColorBrush ellipseColor = new SolidColorBrush();
            ellipseColor.Color = (pinValue) ? Color.FromRgb(0, 255, 0) : Color.FromRgb(255, 0, 0);


            // "górny" pin w kolumnie, trzeba uaktualnić kolor górnej kulki
            if (BoxNumber1.Text.Equals(pinNumber.ToString()))
            {
                EllipseTop.Fill = ellipseColor;
            }
            // "dolny" pin w kolumnie, trzeba uaktualnić kolor dolnej kulki
            else
            {
                EllipseBottom.Fill = ellipseColor;
            }
        }
    }
}
