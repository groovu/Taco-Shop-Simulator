using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            //add reference to db (ADO > select db)
            //init db
            //retrieve row using lambda fxn
            //modify like a normal object
            //profit
            TacoDBEntities taco = new TacoDBEntities();
            var table = taco.testTables;

            var result = table.SingleOrDefault(b => b.id == 1);

            result.blah = "1000";
            taco.SaveChanges();

        }
    }
}
