﻿using System;
using System.Collections.Generic;
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
using MiunskeBoardProject.classes;

namespace MiunskeBoardProject.ui_elements
{
    /// <summary>
    /// Logika interakcji dla klasy PinBottomBoolean.xaml
    /// </summary>
    public partial class PinBottomBoolean : UserControl
    {

        public int canAddress { get; set; }
        public String canBit { get; set; }


        /// <summary>
        /// konstruktor elementu XAML odpowiadającego za pojedyńczą kolumnę widoku konektora
        /// </summary>
        /// <param name="pinNumber">numer pina do wyświetlenia u dołu kolumny</param>
        /// <param name="canAddress">adres CAN na który ta kontrolka reaguje</param>
        /// <param name="canBit">offsety bitu z którego jest czytana i wyświetlana wartość</param>
        public PinBottomBoolean(int pinNumber, int canAddress, String canBit)
        {

            InitializeComponent();
            PinNumberXAML.Text = pinNumber.ToString();
            this.canAddress = canAddress;
            this.canBit = canBit;
            MainWindow.Event_RxCanMessage += new EventHandler<CanFoxRxEventArg>(test);
            //MainWindow.CANMessageEvent += new EventHandler<CANMessage>(can_event_update_pin);
        }


        private void test(object sender, CanFoxRxEventArg e)
        {
            CANMessages msgs = e.CopyOfRxMessages((int)canAddress);

            if (msgs.Count < 1 || msgs.Equals(null))
                return;

            if (canBit.Contains('-'))
            {
                MessageBox.Show("Pin logiczny zawiera w konfiguracji wartość " + canBit + " proszę podać jednocyfrową wartość value");
                Environment.Exit(0);
            }


            foreach (CANMessage msg in msgs)
            {
                int arrayOffset = int.Parse(canBit.ToString()) / 8;
                string binData = Convert.ToString(msg.data[arrayOffset], 2).PadLeft(8, '0');
                int index = int.Parse(canBit.ToString()) % 8;
                int newValue = Convert.ToInt32(binData.Substring(index, 1));


                BooleanEllipseXAML.Dispatcher.Invoke(() =>
                {
                    SolidColorBrush ellipseColor = new SolidColorBrush();
                    ellipseColor.Color = (newValue > 0) ? Color.FromRgb(0, 255, 0) : Color.FromRgb(255, 0, 0);
                    ellipseColor.Freeze();
                    BooleanEllipseXAML.Fill = new SolidColorBrush(ellipseColor.Color);
                });

            }
        }



        /// <summary>
        /// Subskrybent zdarzenia przesyłania wiadomości CAN w MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message">wiadomość CAN</param>
        /*private void can_event_update_pin(object sender, CANMessage message)
        {

             

            if (message.id != canAddress)
                return;

            if (canBit.Contains('-'))
            {
                MessageBox.Show("Pin logiczny zawiera w konfiguracji wartość " + canBit + " proszę podać jednocyfrową wartość value");
                Environment.Exit(0);
            }

            int arrayOffset = int.Parse(canBit.ToString()) / 8;


   


            string binData = Convert.ToString(message.data[arrayOffset], 2).PadLeft(8, '0');
            int index = int.Parse(canBit.ToString()) % 8;
            int newValue = Convert.ToInt32(binData.Substring(index, 1));


            SolidColorBrush ellipseColor = new SolidColorBrush();
            ellipseColor.Color = (newValue > 0) ? Color.FromRgb(0, 255, 0) : Color.FromRgb(255, 0, 0);
            BooleanEllipseXAML.Fill = ellipseColor;
        }*/

    }
}
