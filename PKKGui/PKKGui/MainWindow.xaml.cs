using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace PKKGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BinaryWriter myStreamWriter;
        BinaryReader myStreamReader;
        public MainWindow()
        {
            InitializeComponent();
            TcpClient myClient = new TcpClient();
            myClient.Connect("127.0.0.1", 8080);

            var tcpStream = myClient.GetStream();
            myStreamWriter = new BinaryWriter(tcpStream);
            myStreamReader = new BinaryReader(tcpStream);

            ThreadPool.QueueUserWorkItem(HandleReading, myStreamReader);

        }

        private void HandleReading(object state)
        {
            var myStreamReader = state as BinaryReader;
            do
            {
                try
                {
                    // read the string sent to the server
                    var theReply = myStreamReader.ReadString();
                    Dispatcher.BeginInvoke(new Action(() => {
                        ChatBox.Text += theReply;
                    }));
                }
                catch (Exception)
                {
                    break;
                }
            } while (true);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myStreamWriter.Write(MessageTextbox.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
