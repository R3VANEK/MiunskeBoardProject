using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using MiunskeBoardProject.classes;



namespace MiunskeBoardProject
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static event EventHandler<CanFoxRxEventArg> Event_RxCanMessage;


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
        }



        /// <summary>
        /// Subskyrbent zdarzenia kliknięcia nazyw płytki w oknie głównym, renderuje nową płytkę o takiej nazwie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void switch_Board_Visual(object sender, EventArgs e)
        {
            if (BoardList.SelectedItem == null)
                return;

            boardHolder.Children.Clear();

            string boardClassName = "MiunskeBoardProject.boards." +BoardList.SelectedItem.ToString();

            var a = Activator.CreateInstance(Type.GetType(boardClassName), null);
            boardHolder.Children.Add((UIElement)a);
        }



        private void CANFox_Event_RxCanMessage(object sender, CanFoxRxEventArg e)
        {
            FireEventRxCanMessage(e);
        }

        private void FireEventRxCanMessage(CANMessages newMessages)
        {
            EventHandler<CanFoxRxEventArg> event_RxCanMessage = Event_RxCanMessage;
            if (event_RxCanMessage != null)
            {
                event_RxCanMessage(this, new CanFoxRxEventArg(newMessages));
            }
        }
        private void FireEventRxCanMessage(CanFoxRxEventArg EventMessages)
        {
            EventHandler<CanFoxRxEventArg> event_RxCanMessage = Event_RxCanMessage;
            if (event_RxCanMessage != null)
            {
                event_RxCanMessage(this, EventMessages);
            }
        }


        /// <summary>
        /// Subskrybent zdarzneia kliknięcia przycisku "Otrzymuj iwadomośći CAN"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void can_button_click(object sender, EventArgs e)
        {

            CANFox cf = new CANFox();

            try
            {
                CANFox.ErrorCode res = cf.openCan();

                if(res != CANFox.ErrorCode.NTCAN_SUCCESS)
                {
                    MessageBox.Show(Enum.GetName(typeof(CANFox.ErrorCode), res));
                    return;
                }
                
                CANFox.Event_RxCanMessage += CANFox_Event_RxCanMessage;
            }
            catch(Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
           

        }

    }

    
}
