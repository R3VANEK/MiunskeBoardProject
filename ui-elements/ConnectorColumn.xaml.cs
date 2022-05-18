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
    }
}
