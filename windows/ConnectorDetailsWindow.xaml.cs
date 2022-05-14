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
using MiunskeBoardProject.classes;

namespace MiunskeBoardProject
{
    /// <summary>
    /// Logika interakcji dla klasy ConnectorDetailsWindow.xaml
    /// </summary>
    public partial class ConnectorDetailsWindow : Window
    {

        private ConnectorControl connector { get; set; }


        public ConnectorDetailsWindow(ConnectorControl connector, RootJson configInfo, string configFileName)
        {
            InitializeComponent();
            this.connector = connector;
            connectorNameXAML.Text = "Connector " + this.connector.Name;

            bool hasConfig = false;
            for(int i = 0; i < configInfo.connectors.Count; i++)
            {
                if (configInfo.connectors[i].name == this.connector.Name)
                    hasConfig = true;
            }

            if (!hasConfig)
                connectorErrorXAML.Text = "brak konfiguracji tego konektora w pliku " + configFileName;


            for (double i = connector.Pins; i > 0; i -= 2)
            {
                ConnectorHolderXAML.Children.Add(new ConnectorColumn((int)i, (int)(i-1)));
            }

            ConnectorHolderXAML.Children.Add(new ConnectorColumnSpecial());



            


        }
    }
}
