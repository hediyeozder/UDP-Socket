using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace udpServer {
    public partial class Form1 : Form {
        UdpClient udpServer;
        IPEndPoint serverEndPoint;

        public Form1() {
            InitializeComponent();
        }

        private void Startbutton_Click(object sender, EventArgs e) {
            try {
                string serverIP = ServerIPtextbox.Text;
                int serverPort = int.Parse(ServerPorttextBox.Text);

                serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
                udpServer = new UdpClient(serverEndPoint);

                AddMessageToList($"Server started on {serverIP}:{serverPort}");
                Task.Run(() => ReceiveMessages());
            } catch (Exception ex) {
                MessageBox.Show($"Error starting server: {ex.Message}");
            }
        }

        private async void ReceiveMessages() {
            try {
                while (true) {
                    UdpReceiveResult result = await udpServer.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);
                    string receivedFrom = result.RemoteEndPoint.ToString();

                    AddMessageToList($"Received from {receivedFrom}: {message}");
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error receiving message: {ex.Message}");
            }
        }

        private void AddMessageToList(string message) {
            if (ListBox.InvokeRequired) {
                ListBox.Invoke(new Action<string>(AddMessageToList), message);
            } else {
                ListBox.Items.Add(message);
            }
        }
    }
}
