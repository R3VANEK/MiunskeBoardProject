﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using MiunskeBoardProject.base_models;
using MiunskeBoardProject.classes;

namespace MiunskeBoardProject.boards
{
    /// <summary>
    ///  Klasa zawierająca generyczne metody inicjujące logikę panelu Miunske<br/>
    ///  Przy kopiowaniu tego pliku przy dodawaniu nowego panelu prosimy zmienić parametry:<br/>
    ///  <list type="bullet|number|table">
    ///     <item>
    ///         <term>fullBoardName</term>
    ///         <description>Pełna nazwa panelu</description>
    ///     </item>   
    ///     
    /// <item>
    ///     <term>configFileName</term>
    ///     <description>Nazwa pliku konfiguracyjnego json utworzonego w katalogu MiunskeBoardProject/connector-configurations</description>
    /// </item>
    ///  </list>
    /// </summary>
    public partial class MiunskeG2 : UserControl , BoardInterface
    {
        //to będzie w pętli przesyłane obok canRead
        //public event EventHandler<string> BoardInterface.CANMessageEvent;
        public static event EventHandler<CANMessage> CANMessageEvent;


        public const string fullBoardName = "Miunske A1-1203-3015 DigitalPlatine G2 BL: 0.1.0.8";
        public const string configFileName = "MiunskeG2.json";


        private List<ConnectorControl> connectorControls;
        private RootJson configInfo;

        
        /// <summary>
        /// Konstruktor klasy Panelu<br/>
        /// <list type="bullet">
        ///     <item>
        ///         <description>Inicjuje wygląd XAML</description>
        ///     </item>
        ///     <item>
        ///         <description>waliduje przypisany plik konfiguracyjny json</description>
        ///     </item>
        ///     <item>
        ///         <description>odnajduje wszystkie kontrolery w XAMLU i dopisuje do nich zdarzenie onClick wyświetlajace ConnectorDetailsWindow</description>
        ///     </item>
        /// </list>
        /// </summary>
        public MiunskeG2()
        {
            

            int validateStatus = BoardInterface.validateJsonConfig(configFileName);
            if (validateStatus != 1)
            {
                switch (validateStatus)
                {
                    case -1: MessageBox.Show("niepoprawny układ pliku : " + configFileName + " proszę sprawdzić formatowanie"); break;
                    case -2: MessageBox.Show("neipoprawne formatowanie parametru 'connectors' w pliku " + configFileName); break;
                    case -3: MessageBox.Show("niepoprawne formatowanie atrybutów pojedyńczego 'connectors' w pliku " + configFileName); break;
                    case -4: MessageBox.Show("niepoprawe formatowanie pojedyńczego elementu listy 'pins-parameters w pliku '"+ configFileName); break;
                }
                return;
            }


            connectorControls = new List<ConnectorControl>();
            configInfo = BoardInterface.generateConfig(configFileName);

            InitializeComponent();
            Loaded += new RoutedEventHandler(onLoad);

        }


        public async void simulateCANMessages()
        {
            await System.Threading.Tasks.Task.Delay(4000);
            int i = 0;
            Random r = new Random();
            while (true)
            {
                CANMessage message = new CANMessage(205, 8);
                for (int j = 0; j < message.len; j++)
                    message.aby_data[i] = (char)r.Next(0, 255);

                await System.Threading.Tasks.Task.Delay(6000);
                System.Diagnostics.Trace.WriteLine("can message incoming " + i);
                CANMessageEvent?.Invoke(this, message);
                i += 1;

            }

        }




        /// <summary>
        ///     metoda odnajdująca i przypisująca do wszystkich konnektorów w xamlu onlclicka wyświetlającego ConnectorDetailsWindow
        /// </summary>
        void onLoad(object sender, RoutedEventArgs e)
        {
            foreach (ConnectorControl imageBox in BoardInterface.FindChilds<ConnectorControl>((DependencyObject)sender))
            {
                imageBox.MouseDown += new MouseButtonEventHandler((s,e)=>BoardInterface.clickConnector(s,e,configInfo, configFileName));
                this.connectorControls.Add(imageBox);


                simulateCANMessages();
            }

            
        }

    }
}
