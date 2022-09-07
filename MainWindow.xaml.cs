﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using MiunskeBoardProject.classes;
using System.Runtime.InteropServices;



namespace MiunskeBoardProject
{


   



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       

        //[DllImport("user32.dll", CharSet = CharSet.Unicode)]
        //public unsafe static extern long canOpen(long l_netnumber, long l_mode, long l_echoon, long l_txtimeout, long l_rxtimeout, char* c_ApplicationName, char* c_ReceiverEvent, char* c_ErrorEvent, void** handle);

        public static event EventHandler<CANMessage> CANMessageEvent;




        public MainWindow()
        {
            
            InitializeComponent();

            string[] boardFiles = Directory.GetFiles("../../../boards", "*xaml.cs");

            foreach (string boardName in boardFiles)
            {
                string trimmedString = boardName.Substring(16, boardName.Length - 24);
                BoardList.Items.Add(trimmedString);
            }

            BoardList.SelectionChanged += new SelectionChangedEventHandler(switch_Board_Visual);
            canButton.Click += new RoutedEventHandler(can_button_click);
            /* this.Loaded += new RoutedEventHandler(onLoad); */
        }




        private void switch_Board_Visual(object sender, EventArgs e)
        {
            if (BoardList.SelectedItem == null)
                return;

            boardHolder.Children.Clear();

            string boardClassName = "MiunskeBoardProject.boards." +BoardList.SelectedItem.ToString();

            var a = Activator.CreateInstance(Type.GetType(boardClassName), null);
            boardHolder.Children.Add((UIElement)a);
        }


        private void can_button_click(object sender, EventArgs e)
        {
            // tutaj logika połączenia się z biblioteką can
            // wiadomości czy się udało itp

           // canOpen(105, 0, 0, 4000, 4000, "test", "R1", "E1", &MAINHANDLER)



            MessageBox.Show("przesyłanie wiadomości CAN");
            simulateCANMessages(205,8);
            simulateCANMessages(203, 8);
            simulateCANMessages(211, 8);
        }



        /// <summary>
        /// Symuluj przesyłanie wiadomości CAM
        /// </summary>
        /// <param name="canAddress">adres CAN</param>
        /// <param name="len">długość wiadomości 1-8</param>
        public async void simulateCANMessages(int canAddress, int len)
        {
            await System.Threading.Tasks.Task.Delay(4000);
            int i = 0;
            Random r = new Random();
            while (true)
            {
                CANMessage message = new CANMessage(canAddress, len);
                for (int j = 0; j < message.len; j++)
                    message.aby_data[j] = (char)r.Next(0, 254);

                await System.Threading.Tasks.Task.Delay(6000);
                System.Diagnostics.Trace.WriteLine("can message incoming " + i);
                MainWindow.CANMessageEvent?.Invoke(this, message);
                i += 1;

            }

        }












    }

    
}
