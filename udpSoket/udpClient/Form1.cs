using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace udpClient {
    public partial class Form1 : Form {
        UdpClient udpClient;
        IPEndPoint clientEndPoint;

        public Form1() {
            InitializeComponent();
        }

        private void Connectbutton_Click(object sender, EventArgs e) {
            try {
                string clientIP = "localhost";
                int clientPort = int.Parse(ClientPorttextBox.Text);

                clientEndPoint = new IPEndPoint(IPAddress.Parse(clientIP), clientPort);
                udpClient = new UdpClient(clientEndPoint);

                AddMessageToList($"Client connected from {clientIP}:{clientPort}");
                Task.Run(() => ReceiveMessages());
            } catch (Exception ex) {
                MessageBox.Show($"Error connecting client: {ex.Message}");
            }
        }

        private async void Sendbutton_Click(object sender, EventArgs e) {
            try {
                string message = MessagetextBox.Text;
                byte[] data = Encoding.UTF8.GetBytes(message);

                string serverIP = ServerIPtextbox.Text;
                int serverPort = int.Parse(ServerPorttextBox.Text);

                await udpClient.SendAsync(data, data.Length, serverIP, serverPort);
                AddMessageToList($"Sent to {serverIP}:{serverPort} - {message}");
                MessagetextBox.Clear();
            } catch (Exception ex) {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }

        private async void ReceiveMessages() {
            try {
                while (true) {
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
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
