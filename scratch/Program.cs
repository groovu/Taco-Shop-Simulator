using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scratch
{
    class Program
    {
        //NOTE
        // everytime you update the db from SSMS, you have to readd db to update.  Is there a way to do this without having to readd refs?
        // nvm, you can go to the db model designer and add tables and update from there.
        static void Main(string[] args)
        {
            //add reference to db (ADO > select db)
            //init db
            //retrieve row using lambda fxn
            //modify like a normal object
            //profit
            //tacodbentity taco = new tacodbentity();
            //var table = taco.logins;

            //var result = table.singleordefault(b => b.id == 1);

            //result.blah = "1000";
            //taco.SaveChanges();

            string username;
            string password;
            Console.WriteLine("Enter username:");
            username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            password = Console.ReadLine();

           // var userTable = taco.users;
        }
    }
}
