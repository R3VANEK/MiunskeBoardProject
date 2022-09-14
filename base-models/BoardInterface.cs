using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using MiunskeBoardProject.classes;
using Newtonsoft.Json;
using System.IO;


namespace MiunskeBoardProject.base_models
{
    /// <summary>
    ///     Zawiera metody niezbędne do symulowania płytek
    /// </summary>
    interface BoardInterface
    {

        
        

        /// <summary>
        ///     metoda obsługująca kliknięcie na element graficzny odpowiadający konektorowi<br/>
        ///     uruchamia nowe okno ConnectorDetailsWindow pokazujące szczegółu sygnałów konektora
        /// </summary>
        protected static void clickConnector(object sender, System.Windows.Input.MouseButtonEventArgs e, Root configInfo, string configFileName)
        {

            ConnectorControl connector = (ConnectorControl)sender;
           

            ConnectorDetailsWindow cw = new ConnectorDetailsWindow(connector, configInfo, configFileName);
            cw.ShowInTaskbar = false;
            cw.Owner = Application.Current.MainWindow;
            cw.Show();
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
