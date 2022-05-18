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
using MiunskeBoardProject.boards;
using static MiunskeBoardProject.classes.JsonParser;

namespace MiunskeBoardProject
{
    /// <summary>
    /// klasa ConnectorDetailsWindow odpowiada za pokazywanie wizualizacji poszczególnych konektorów i ich reakcji na wiadomości CAN<br/>
    /// po kliknięciu na element XAML, metoda BoardInterface.clickConnector tworzy obiekt tej klasy
    /// na podstawie uzupełnienia parametrów kontrolki ConnectorControl w XAMLU generowany jest wygląd i nazwa okna
    /// </summary>
    public partial class ConnectorDetailsWindow : Window
    {

        private ConnectorControl connectorControl { get; set; }
        private Connector connectorConfig { get; set; }


        /// <summary>
        /// Jedyny konstruktor klasy. Generuje dynamicznie wygląd i sprawdza czy opisano zachowania pinów tego konektora w pliku json<br/>
        /// Jeżeli nie, okno jest puste i nie reaguje na wiadomości CAN
        /// </summary>
        /// <param name="connector">element XAML z którego generuje się obiekt</param>
        /// <param name="configInfo">sparsowany plik konfiguracyjny panelu na którym jest konektor</param>
        /// <param name="configFileName">nazwa pliku konfiguracyjnego panelu</param>
        public ConnectorDetailsWindow(ConnectorControl connector, Root configInfo, string configFileName)
        {
            InitializeComponent();


            

            this.connectorControl = connector;
            connectorNameXAML.Text = "Connector " + this.connectorControl.Name;

            bool hasConfig = false;
            for(int i = 0; i < configInfo.Connectors.Count; i++)
            {
                if (configInfo.Connectors[i].Name == this.connectorControl.Name)
                {
                    hasConfig = true;
                    connectorConfig = configInfo.Connectors[i];
                }
                    
            }

            if (!hasConfig)
            {
                connectorErrorXAML.Text = "brak konfiguracji tego konektora w pliku " + configFileName;
                return;
            }


            PinsParameter pinTest;
            for (int i = connectorConfig.PinsParameters.Count-1; i > 0; i-=2)
            {
                pinTest = connectorConfig.PinsParameters[i];

                if(pinTest.Type == "boolean")
                {
                    ConnectorHolderXAML.Children.Add(new ConnectorColumn(i+1, i));
                }
                else if(pinTest.Type == "value")
                {
                    ConnectorHolderXAML.Children.Add(new ConnectorColumnValue(i+1, i));
                }
                else
                {
                    connectorNameXAML.Text = "Nieprawidłowa wartość 'type' w pinie " + pinTest.Pin;
                }
            }


            /*for (double i = connector.Pins; i > 0; i -= 2)
            {
               
                ConnectorHolderXAML.Children.Add(new ConnectorColumn((int)i, (int)(i-1)));
            }*/


            ConnectorHolderXAML.Children.Add(new ConnectorColumnSpecial());

            MiunskeG2.CANMessageEvent += MiunskeG2_CANMessageEvent;
        }


        /// <summary>
        ///     Subskrybent zdarzenia odczytania nowych wiadomości CAN<br/>
        ///     Na podstawie pliku konfiguracyjnego uaktualnia graficzną wizualizację pinów
        /// </summary>
        /// <param name="sender">obiekt wyzwalający event</param>
        /// <param name="message">wiadomość CAN</param>
        private void MiunskeG2_CANMessageEvent(object sender, CANMessage message)
        {
            //System.Diagnostics.Trace.WriteLine("can message " + e + " read in details window");
            // zgodnie z jsonem rozdysponuj plusy i minusy

            if(!connectorConfig.CanAddresses.Contains(message.address)){
                return;
            }

            for(int i = 0; i < connectorConfig.PinsParameters.Count; i++)
            {
                // każdy message.aby_data[i] to int (0-255)
                // kiedy mają się zaświecić czerwone indykatory a kiedy zielone?
                // może zielone zawsze jeżeli aby_data[i] > 0?
            }



        }

        

    }
}
