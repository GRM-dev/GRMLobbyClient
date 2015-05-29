using Newtonsoft.Json;
using GRMLobbyClient.XAML.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GRMLobbyClient.Core.Connection
{
    public static class PacketParser
    {
        public static string receivePacket(Socket clientSocket)
        {
            if (!clientSocket.Connected)
            {
                throw new IOException("You are not connected!");
            }
            NetworkStream stream = new NetworkStream(clientSocket);
            StringBuilder str = new StringBuilder();
            while (true)
            {
                if (stream.DataAvailable)
                {
                    int b = stream.ReadByte();
                    if (b > 0)
                    {
                        str.Append((char)b);
                    }
                    else
                    {
                        break;
                    }
                }
                else if (str.ToString().Length > 0)
                {
                    break;
                }
            }
            return str.ToString();
        }

        public static bool sendPacket(string msg, Socket clientSocket)
        {
            if (!clientSocket.Connected)
            {
                throw new IOException("You are not connected!");
            }
            if (msg.Length > 0)
            {
                try
                {
                    NetworkStream stream = new NetworkStream(clientSocket);
                    byte[] outBMsg = Encoding.UTF8.GetBytes(msg);
                    byte[] outB = new byte[outBMsg.Length + 4];
                    byte[] outLenB = System.BitConverter.GetBytes(msg.Length + 4);
                    outLenB.CopyTo(outB, 0);
                    outBMsg.CopyTo(outB, 4);
                    stream.Write(outB, 0, outB.Length);
                    return true;
                }
                catch (IOException e)
                {
                    CDialogManager.ShowExceptionDialog(e, "Error while sending msg");
                }
            }
            return false;
        }
    }
}
