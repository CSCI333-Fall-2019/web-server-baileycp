using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebServer
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        

        public Form1()
        {
            InitializeComponent();
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            ThreadPool.QueueUserWorkItem(listenerFunc, this); // keeps UI responsive
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private bool _isRunning;
        static readonly object _isRunningLock = new object();
        private int port = 1234;

        public bool IsRunning
        {
            get
            {
                lock (_isRunningLock)
                {
                    return this._isRunning;
                }
            }
            set
            {
                lock (_isRunningLock)
                {
                    this._isRunning = value;
                }
                button1.Enabled = !value;
                button2.Enabled = value;
            }
        }

        private void listenerFunc(object tempInfo)
        {
            Form1 main = (Form1)tempInfo;
            main.listener = new TcpListener(IPAddress.Any, main.port);
            main.listener.Start();

            while (main.IsRunning)
            {
                try
                {
                    TcpClient client = main.listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(GetRequestedItem, client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error received: " + ex.Message);
                }
            }
        }

        private void GetRequestedItem(object state)
        {
            TcpClient client = (TcpClient)state;

        }
    }
}
