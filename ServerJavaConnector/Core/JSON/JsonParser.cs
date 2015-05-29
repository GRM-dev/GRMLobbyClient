using Newtonsoft.Json;
using GRMLobbyClient.Core.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GRMLobbyClient.Core.JSON
{
    public class JsonParser
    {
        public static void sendData(Object obj, Socket socket, int action = 0)
        {
            String objS = "{\"type\":\"" + obj.GetType().Name + "\",";
            objS += "\"action\":" + action + ",";
            objS += "\"object\": " + JsonConvert.SerializeObject(obj) + "}";
            Console.WriteLine("sending: " + objS);
            PacketParser.sendPacket(objS, socket);
        }

        public static User receiveUserData(int ID, Socket socket)
        {
            PacketParser.sendPacket("!userdata " + ID, socket);
            String rec = "";
            while (!rec.StartsWith("{\""))
            {
                rec = PacketParser.receivePacket(socket);
            }
            try
            {
                Console.WriteLine("received: " + rec);
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
