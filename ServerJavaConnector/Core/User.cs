using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRMLobbyClient.Core
{
    public class User
    {
        public User()
            : this(-1, "NaN", -1, "NaN@NaN.NaN")
        {
        }
        public User(int ID, String name, int age, String mail)
        {
            this.ID = ID;
            this.Name = name;
            this.Age = age;
            this.Mail = mail;
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Mail { get; set; }
    }
}
