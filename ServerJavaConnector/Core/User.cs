using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerJavaConnector.Core
{
    public class User
    {

        public User(int ID, String name, int age, String mail)
        {
            this.ID = ID;
            this.Name = name;
            this.Age = age;
            this.Mail = mail;
        }

        public User()
        {
            // TODO: Complete member initialization
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Mail { get; set; }
    }
}
