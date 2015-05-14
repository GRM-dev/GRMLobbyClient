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

        public static void sendUserData(User user, Socket socket)
        {
            String objS = JsonConvert.SerializeObject(user);
            Console.WriteLine("send " + objS);
            sendPacket(objS, socket);
        }

        public static User receiveUserData(int ID, Socket socket)
        {
            sendPacket("!userdata " + ID, socket);
            String rec = "";
            while (!rec.StartsWith("{\""))
            {
                rec = receivePacket(socket);
            }
            try
            {
                Console.WriteLine("rec " + rec);
                User user = JsonConvert.DeserializeObject<User>(rec);
                Console.WriteLine(user.Name);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
