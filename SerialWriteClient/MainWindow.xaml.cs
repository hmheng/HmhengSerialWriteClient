using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
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
using System.Windows.Threading;

namespace SerialWriteClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static bool _continue;
        static SerialPort _serialPort;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private int speed;
        public MainWindow()
        {
            InitializeComponent();
            worker.DoWork += worker_DoWork;
            setSpeed();

            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = "COM3";
            _serialPort.BaudRate = _serialPort.BaudRate;
            _serialPort.Parity = _serialPort.Parity;
            _serialPort.DataBits = _serialPort.DataBits;
            _serialPort.StopBits = _serialPort.StopBits;
            _serialPort.Handshake = _serialPort.Handshake;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Write();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

            btnConnect.IsEnabled = false;
            btnDisconnect.IsEnabled = true;

            _serialPort.Open();
            _continue = true;
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {

            _continue = false;
            btnConnect.IsEnabled = true;
            btnDisconnect.IsEnabled = false;
            _serialPort.Close();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (_continue)
            {
                _continue = false;
                btnSend.Content = "Demo Start";
            }
            else
            {
                _continue = true;
                worker.RunWorkerAsync();
                btnSend.Content = "Demo Stop";
            }

        }

        public void Write()
        {
            int i = 1;
            while (_continue)
            {
                _serialPort.WriteLine(i.ToString());
                System.Threading.Thread.Sleep(speed);
                
                i++;
                if (i == 4)
                {
                    i = 1;
                }
                
            }
        }
        
        // Display Port values and prompt user to enter a port.
        public static string SetPortName(string defaultPortName)
        {
            string portName = defaultPortName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            //Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            //portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }
        // Display BaudRate values and prompt user to enter a value.
        public static int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        // Display PortParity values and prompt user to enter a value.
        public static Parity SetPortParity(Parity defaultPortParity)
        {
            string parity;

            Console.WriteLine("Available Parity options:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
            parity = Console.ReadLine();

            if (parity == "")
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity, true);
        }
        // Display DataBits values and prompt user to enter a value.
        public static int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
            dataBits = Console.ReadLine();

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits.ToUpperInvariant());
        }

        // Display StopBits values and prompt user to enter a value.
        public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine("Available StopBits options:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter StopBits value (None is not supported and \n" +
             "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
            stopBits = Console.ReadLine();

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }
        public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handshake;

            Console.WriteLine("Available Handshake options:");
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());
            handshake = Console.ReadLine();

            if (handshake == "")
            {
                handshake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
        }

        private void sliderSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            setSpeed();
        }
        private void setSpeed()
        {

            speed = (int)sliderSpeed.Value;
            lblSpeed.Text = speed.ToString();
        }
    }
}
