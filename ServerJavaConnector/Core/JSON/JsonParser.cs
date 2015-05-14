using Newtonsoft.Json;
using ServerJavaConnector.Core.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core.JSON
{
    public class JsonParser
    {
        public static void sendUserData(User user, Socket socket)
        {
            String objS = JsonConvert.SerializeObject(user);
            Console.WriteLine("send " + objS);
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
