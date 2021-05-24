using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using PanelAdmin.model;

namespace PanelAdmin.viewModel
{
    public class ViewModel
    {
        // --- Attributes ---
        public Model model { get; set; }
        public bool isDeconnected { get; set; }
        private Socket server { get; set; }


        // --- Constructor ---
        public ViewModel()
        {
            this.model = new Model();
            isDeconnected = false;
            server = null;
        }

        // --- Methods ---
        private static Socket Connect(string _serverIP, int _serverPort)
        {
            // Create a listener socket
            Socket ConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse(_serverIP);
            IPEndPoint endPoint = new IPEndPoint(address, _serverPort);
            try
            {
                // Connect to the server
                ConnectionSocket.Connect(endPoint);

                Trace.WriteLine($"Client connected to {_serverIP}");

                return ConnectionSocket;
            } catch (SocketException)
            {
                MessageBox.Show(Langs.Lang.secretTunnel);
                return null;
            }
        }

        private void Listen(Socket server)
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[4096];
                    int receivedBytes = server.Receive(buffer);

                    Trace.WriteLine($"Buffer : {receivedBytes}");


                    string MsgReceived = Encoding.Default.GetString(buffer, 0, receivedBytes);
                    Trace.WriteLine($"Buffer : {MsgReceived}");
                    while (MsgReceived != "")
                    {
                        int msgLength = MsgReceived.IndexOf("]") + 1;
                        ObservableCollection<Work> jsonList = JsonSerializer.Deserialize<ObservableCollection<Work>>(MsgReceived.Substring(0, msgLength));
                        if (jsonList.Count >= this.model.works.Count)
                        {
                            for (int i = 0; i < jsonList.Count; i++)
                            {
                                if (this.model.works.Count > i && jsonList[i].name == this.model.works[i].name)
                                {
                                    this.model.works[i].colorProgressBar = jsonList[i].colorProgressBar;
                                    this.model.works[i].progress = jsonList[i].progress;
                                }
                                else
                                {
                                    this.model.works = jsonList;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            int j = 0;

                            for (int i = 0; i < this.model.works.Count; i++)
                            {
                                if (this.model.works[j].name == jsonList[i].name)
                                {
                                    this.model.works[j].colorProgressBar = jsonList[i].colorProgressBar;
                                    this.model.works[j].progress = jsonList[i].progress;
                                }
                                else
                                {
                                    this.model.works = jsonList;
                                    break;
                                }
                                j++;
                            }
                        }

                        MsgReceived = MsgReceived.Length > msgLength + 1 ? MsgReceived.Substring(msgLength) : "";
                    }
                }
            }
            catch (SocketException)
            {
                MessageBox.Show(Langs.Lang.socketDeconnection);
            }

        }

        public void SendAction(string _action, int[] id)
        {
            object SendObject = new
            {
                action = _action,
                selectedId = id
            };
            string jsonObject = JsonSerializer.Serialize(SendObject);
            byte[] buffer = Encoding.Default.GetBytes(jsonObject);
            this.server.Send(buffer);
        }

        public void Deconnect(Socket socket)
        {
            socket.Close();
        }

        public void Connection(string _serverIP, int _serverPort)
        {
            this.server = Connect(_serverIP, _serverPort);
            if(server != null)
            {
                Listen(this.server);
            }
        }
    }
}
