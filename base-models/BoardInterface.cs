using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MiunskeBoardProject.base_models
{
    interface BoardInterface
    {
       

        protected static void clickConnector(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            ConnectorControl connector = (ConnectorControl)sender;
            double numberOfPins = connector.Pins;

            if (numberOfPins == 0)
            {
                //TODO: Wywal aplikacje bo to błąd krytyczny albo pomiń
            }

            ConnectorDetailsWindow cw = new ConnectorDetailsWindow(connector);
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
