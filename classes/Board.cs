using MiunskeBoardProject.base_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using MiunskeBoardProject.classes;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiunskeBoardProject.classes
{
    class Board : UserControl
    {
        /// <summary>
        /// Pełna nazwa płyty głównej wyświetlana w apliakcji
        /// </summary>
        public string fullBoardName { get; set; }

        /// <summary>
        /// nazwa pliku JSON w którym znajudją się informacje jak płyta reaguje na wiadomości CAN
        /// </summary>
        public string configFileName { get; set; }


        public static event EventHandler<CANMessage> CANMessageEvent;



        public Board(String fullBoardName, String configFileName)
        {
            this.fullBoardName = fullBoardName;
            this.configFileName = configFileName;

            try
            {
                jsonParser = new JsonParser(configFileName);
            }
            catch (JsonSerializationException e)
            {
                MessageBox.Show(e.Message);
            }


            connectorControls = new List<ConnectorControl>();
        }

        private List<ConnectorControl> connectorControls;
        private JsonParser jsonParser;



        /// <summary>
        /// Pomocnicza metoda znajdująca "dzieci" obiektu w xaml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depObj"></param>
        /// <returns></returns>
        protected static IEnumerable<T> FindChilds<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindChilds<T>(ithChild)) yield return childOfChild;
            }
        }


        /// <summary>
        /// metoda reagująca na kliknięcie konektorów w xamlu. Renderuje nowe okno ze szczegółami konektora
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="configInfo"></param>
        /// <param name="configFileName"></param>
        protected static void clickConnector(object sender, System.Windows.Input.MouseButtonEventArgs e, Root configInfo, string configFileName)
        {

            ConnectorControl connector = (ConnectorControl)sender;
            double numberOfPins = connector.Pins;

            if (numberOfPins == 0)
            {
                //TODO: Wywal aplikacje bo to błąd krytyczny albo pomiń
            }

            ConnectorDetailsWindow cw = new ConnectorDetailsWindow(connector, configInfo, configFileName);
            cw.ShowInTaskbar = false;
            cw.Owner = Application.Current.MainWindow;
            cw.Show();
        }





        /// <summary>
        /// metoda do celów testowych aplikacji, symuluje wysyłanie wiadomości CAN
        /// </summary>
        public async void simulateCANMessages()
        {
            await System.Threading.Tasks.Task.Delay(4000);
            int i = 0;
            Random r = new Random();
            while (true)
            {
                CANMessage message = new CANMessage(205, 8);
                for (int j = 0; j < message.len; j++)
                    message.aby_data[j] = (char)r.Next(0, 254);

                await System.Threading.Tasks.Task.Delay(6000);
                System.Diagnostics.Trace.WriteLine("can message incoming " + i);
                CANMessageEvent?.Invoke(this, message);
                i += 1;

            }

        }

    }
}
