using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSSWpf
{
    class User
    {
        public int id { get; set; }
        public DateTime creation { get; set; }
        public string username { get; set; }
        public decimal money { get; set; }
        public int employees {get; set;}
        public string recipes { get; set; }
        public string shopName { get; set; }

        public User(int id, string username) //new User constructor
        {
            this.id = id;
            creation = DateTime.Now;
            this.username = username;
            money = 1000;
            employees = 0;
        }
        public User()
        {

        }
    }
}
