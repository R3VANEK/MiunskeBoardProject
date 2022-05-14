using System;
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

namespace MiunskeBoardProject.boards
{
    /// <summary>
    /// Logika interakcji dla klasy MiunskeG2.xaml
    /// </summary>
    public partial class MiunskeG2 : UserControl , BoardInterface
    {


        public const string fullBoardName = "Miunske A1-1203-3015 DigitalPlatine G2 BL: 0.1.0.8";


        private List<ConnectorControl> connectorControls;

       



        public MiunskeG2()
        {
            connectorControls = new List<ConnectorControl>();

            InitializeComponent();
            Loaded += new RoutedEventHandler(onLoad);

        }







        /// <summary>
        /// code that connects every connectorControl with onClick event
        /// </summary>
        void onLoad(object sender, RoutedEventArgs e)
        {
            foreach (ConnectorControl imageBox in BoardInterface.FindChilds<ConnectorControl>((DependencyObject)sender))
            {
                imageBox.MouseDown += new MouseButtonEventHandler(BoardInterface.clickConnector);
                this.connectorControls.Add(imageBox);
            }
        }

    }
}
