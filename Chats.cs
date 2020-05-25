using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Next_Generation_School_System_by_Anton {
    public partial class Chats : Form {
        int y = 12;
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Chats() {
            InitializeComponent();
        }

        private void Chats_Load(object sender, EventArgs e) {

        }

        private void timer_Tick(object sender, EventArgs e) {
            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, 11));
                socket.Listen(25);
                socket.BeginAccept(AcceptCallBack, null);
            }
            finally { }
        }
        void AcceptCallBack(IAsyncResult ar) {
            Socket client = socket.EndAccept(ar);
            Thread thread = new Thread(HandleClient);
            thread.Start(client);
        }

        void HandleClient(object o) {
            Socket client = (Socket)o;
            byte[] message = new byte[2042];
            client.Receive(message);
        }

        private void button1_Click(object sender, EventArgs e) {

            Label lbl = new Label();
            lbl.Text = textBox1.Text;
            lbl.Location = new Point(12, y);
            lbl.Size = new Size(776, 26);
            panel1.Controls.Add(lbl);
            y += 30;
        }
    }
}
