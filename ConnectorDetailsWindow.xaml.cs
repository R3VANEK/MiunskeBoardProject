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
using System.Windows.Shapes;
using MiunskeBoardProject.base_models;
using MiunskeBoardProject.ui_elements;

namespace MiunskeBoardProject
{
    /// <summary>
    /// Logika interakcji dla klasy ConnectorDetailsWindow.xaml
    /// </summary>
    public partial class ConnectorDetailsWindow : Window
    {

        private ConnectorControl connector { get; set; }


        public ConnectorDetailsWindow(ConnectorControl connector)
        {
            InitializeComponent();
            this.connector = connector;
            connectorNameXAML.Text = "Connector " + this.connector.Name;

            for(double i = connector.Pins; i > 0; i -= 2)
            {
                ConnectorHolderXAML.Children.Add(new ConnectorColumn((int)i, (int)(i-1)));
            }

            ConnectorHolderXAML.Children.Add(new ConnectorColumnSpecial());

            


        }
    }
}
