using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.Connection
{
    public static class PacketParser
    {
        public static string receivePacket(Socket clientSocket)
        {
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

        public static void sendPacket(string msg, Socket clientSocket)
        {
            if (msg.Length > 0)
            {
                NetworkStream stream = new NetworkStream(clientSocket);
                byte[] outBMsg = Encoding.UTF8.GetBytes(msg);
                byte[] outB = new byte[outBMsg.Length + 4];
                byte[] outLenB = System.BitConverter.GetBytes(msg.Length + 4);
                outLenB.CopyTo(outB, 0);
                outBMsg.CopyTo(outB, 4);
                stream.Write(outB, 0, outB.Length);
            }
        }
    }
}
