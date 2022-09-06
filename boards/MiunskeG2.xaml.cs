using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MiunskeBoardProject.base_models;
using MiunskeBoardProject.classes;
using Newtonsoft.Json;

namespace MiunskeBoardProject.boards
{
    /// <summary>
    ///  Klasa zawierająca generyczne metody inicjujące logikę panelu Miunske<br/>
    ///  Przy kopiowaniu tego pliku przy dodawaniu nowego panelu trzeba zmienić parametry:<br/>
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

        public const string fullBoardName = "Miunske A1-1203-3015 DigitalPlatine G2 BL: 0.1.0.8";
        public const string configFileName = "MiunskeG2.json";


        private List<ConnectorControl> connectorControls;
        private JsonParser jsonParser;

        
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

            try
            {
                jsonParser = new JsonParser(configFileName);
            }
            catch(JsonSerializationException e)
            {
                MessageBox.Show(e.Message);
            }
            
            
            connectorControls = new List<ConnectorControl>();

            InitializeComponent();
            Loaded += new RoutedEventHandler(onLoad);

        }


        /// <summary>
        ///     metoda odnajdująca i przypisująca do wszystkich konnektorów w xamlu onclicka wyświetlającego ConnectorDetailsWindow
        /// </summary>
        void onLoad(object sender, RoutedEventArgs e)
        {
            foreach (ConnectorControl imageBox in BoardInterface.FindChilds<ConnectorControl>((DependencyObject)sender))
            {
                imageBox.MouseDown += new MouseButtonEventHandler((s,e)=>BoardInterface.clickConnector(s,e,jsonParser.getConfigInfo(), configFileName));
                this.connectorControls.Add(imageBox);

            }
        }

    }
}
