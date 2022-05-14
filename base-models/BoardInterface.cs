using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using MiunskeBoardProject.classes;
using Newtonsoft.Json;
using System.IO;
using MiunskeBoardProject.classes;

namespace MiunskeBoardProject.base_models
{
    interface BoardInterface
    {
       

        protected static void clickConnector(object sender, System.Windows.Input.MouseButtonEventArgs e, RootJson configInfo, string configFileName)
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
        /// Waliduje poprawności pliku konfiguracyjnego json przypisanego do płyty
        /// </summary>
        /// <returns>
        ///     1 - poprawny plik json<br/>
        ///     -1 - nie udało się otworzyć pliku<br/>
        ///     -2 - atrybut 'connectors' niepoprawnie sformatowany<br/>
        ///     -3 - atrybuty pojedyńczego 'connectors' niepoprawnie sformatowane<br/>
        ///     -4 - atrybuty pojedyńczego 'pins-parameters' niepoprawnie sformatowane<br/>
        /// </returns>
        protected static int validateJsonConfig(string jsonFileName)
        {

            try
            {
                string raw = File.ReadAllText("../../../connector-configurations/" + jsonFileName);
                RootJson config = JsonConvert.DeserializeObject<RootJson>(raw);

                if (config.connectors == null)
                    return -2;

                foreach(ConnectorJSON conn in config.connectors)
                {
                    if (conn.name == null || conn.pins == null || conn.PinsParameters == null)
                        return -3;

                    foreach(PinsParameterJSON param in conn.PinsParameters)
                        if (param.pin == null || param.CanBit == null)
                            return -4;
                    
                }
            }
            catch (Exception)
            {
                return -1;
            }

            return 1;
        }


        protected static RootJson generateConfig(string jsonFileName)
        {
            string raw = File.ReadAllText("../../../connector-configurations/" + jsonFileName);
            return JsonConvert.DeserializeObject<RootJson>(raw);
        } 



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
    }
}
