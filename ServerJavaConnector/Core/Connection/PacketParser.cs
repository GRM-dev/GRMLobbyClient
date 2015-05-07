using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.Connection
{
    public static class PacketParser
    {
        private static string receivePacket(Socket clientSocket)
        {
            byte[] rcvLenBytes = new byte[4];
            String rcv = "";
            try
            {
                clientSocket.ReceiveTimeout = 100;
                clientSocket.Receive(rcvLenBytes);
                int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
                byte[] rcvBytes = new byte[rcvLen];
                clientSocket.Receive(rcvBytes);
                rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("respond after a period of time"))
                {
                    Console.Out.WriteLine("Exception while receiving msg:\n" + e.Message);
                }
            }
            return rcv;
        }

        private static void sendPacket(String msg, Socket clientSocket)
        {
            int msgLen = System.Text.Encoding.ASCII.GetByteCount(msg);
            byte[] msgBytes = System.Text.Encoding.ASCII.GetBytes(msg);
            byte[] msgLenBytes = System.BitConverter.GetBytes(msgLen);
            clientSocket.Send(msgLenBytes);
            clientSocket.Send(msgBytes);
        }

        public static void sendMessage(String msg, Socket clientSocket)
        {
            if (!msg.StartsWith("!msg "))
            {
                msg = "!msg " + msg;
            }
            sendPacket(msg, clientSocket);
        }

        public static string receiveMessage(Socket clientSocket)
        {
            String rec = receivePacket(clientSocket);
            if (rec.StartsWith("!msg ")) //TODO: commands on '!' mark
            {
                return rec;
            }
            return rec;
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
