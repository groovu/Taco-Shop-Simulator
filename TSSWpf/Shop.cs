using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSSWpf
{
    class Shop
    {
        public User Owner { get; set; }
        public string ShopName { get; set; }
        public Dictionary<ingredient, int> stock = new Dictionary<ingredient, int>();
        //public List<Employee> employees = new List<Employee>();
        public int employees;
        public List<recipe> recipes = new List<recipe>();
        
        public Shop()
        {

        }
    }
}
