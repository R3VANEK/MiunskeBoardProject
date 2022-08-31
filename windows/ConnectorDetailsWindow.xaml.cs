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

        private ConnectorControl ConnectorControl { get; set; }
        private Connector connectorConfig { get; set; }

        private List<PinTopBoolean> pinTopBooleans { get; set; }
        private List<PinBottomBoolean> pinBottomBooleans { get; set; }
        
        private List<PinTopValue> pinTopValues { get; set; }
        private List<PinBottomValue> pinBottomValues { get; set; }

        private List<UserControl> pinXAMLControls { get; set; }


        protected static event EventHandler<CANMessage> CANMessageEvent;


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

            this.pinTopBooleans = new List<PinTopBoolean>();
            this.pinBottomBooleans = new List<PinBottomBoolean>();
            this.pinTopValues = new List<PinTopValue>();
            this.pinBottomValues = new List<PinBottomValue>();
            this.pinXAMLControls = new List<UserControl>();

            this.ConnectorControl = connector;
            connectorNameXAML.Text = "Connector " + this.ConnectorControl.Name;

            bool hasConfig = false;
            for(int i = 0; i < configInfo.Connectors.Count; i++)
            {
                if (configInfo.Connectors[i].Name == this.ConnectorControl.Name)
                {
                    hasConfig = true;
                    connectorConfig = configInfo.Connectors[i];
                    connectorConfig.PinsXAMLObjects = new List<UserControl>();
                }
                    
            }

            if (!hasConfig)
            {
                connectorErrorXAML.Text = "brak konfiguracji tego konektora w pliku " + configFileName;
                return;
            }

            // konfiguracja górnego pina
            PinsParameter pinParam;

            // konfiguracja dolnego pina
            PinsParameter pinParam1;

            connectorConfig.PinsParameters.Sort((x, y) => x.Pin.CompareTo(y.Pin));


            for(int i = connectorConfig.PinsParameters.Count-1; i > 0; i -= 2)
            {
               
                pinParam = connectorConfig.PinsParameters[i];
                pinParam1 = connectorConfig.PinsParameters[i - 1];
                
                StackPanel columnHolderPanel = new StackPanel();
                columnHolderPanel.Orientation = Orientation.Vertical;
                
                if(pinParam.Type == "boolean")
                {
                    PinTopBoolean ptb = new PinTopBoolean(pinParam.Pin);
                    //this.pinTopBooleans.Add(ptb);
                    connectorConfig.PinsXAMLObjects.Add(ptb);
                    //this.pinXAMLControls.Add(ptb);
                    columnHolderPanel.Children.Add(ptb);
                }
                else if(pinParam.Type == "value")
                {
                    PinTopValue ptv = new PinTopValue(pinParam.Pin);
                    //this.pinTopValues.Add(ptv);
                    //this.pinXAMLControls.Add(ptv);
                    connectorConfig.PinsXAMLObjects.Add(ptv);
                    columnHolderPanel.Children.Add(ptv);
                }


                if(pinParam1.Type == "boolean")
                {
                    PinBottomBoolean pbb = new PinBottomBoolean(pinParam1.Pin);
                    //this.pinBottomBooleans.Add(pbb);
                    //this.pinXAMLControls.Add(pbb);
                    connectorConfig.PinsXAMLObjects.Add(pbb);
                    columnHolderPanel.Children.Add(pbb);
                }
                else if(pinParam1.Type == "value")
                {
                    PinBottomValue pbv = new PinBottomValue(pinParam1.Pin);
                   // this.pinXAMLControls.Add(pbv);
                    connectorConfig.PinsXAMLObjects.Add(pbv);
                    columnHolderPanel.Children.Add(pbv);
                }

                ConnectorHolderXAML.Children.Add(columnHolderPanel);
               
            }


            ConnectorHolderXAML.Children.Add(new ConnectorColumnSpecial());

            MiunskeG2.CANMessageEvent += MiunskeG2_CANMessageEvent; 

            
        }


        /// <summary>
        ///     Subskrybent zdarzenia odczytania nowych wiadomości CAN<br/>
        ///     Na podstawie pliku konfiguracyjnego uaktualnia graficzną wizualizację pinów
        ///     Za każdym razem wywołuje dla wszystkich Pinów ich metodę updatePinValue
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

            //tutaj foreach dla każdego PinBottom i PinTop z wywołaniem metod updatePinValue
            for(int i = 0; i < message.aby_data.Length; i++)
            {

            }

            for(int i = 0; i < connectorConfig.PinsParameters.Count; i++)
            {
                PinsParameter pinsParameter = connectorConfig.PinsParameters[i];
                int messageVal = message.aby_data[i];





                // każdy message.aby_data[i] to int (0-255)
                // kiedy mają się zaświecić czerwone indykatory a kiedy zielone?
                // może zielone zawsze jeżeli aby_data[i] > 0?
            }



        }

        

    }
}
