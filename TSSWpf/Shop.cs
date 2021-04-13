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
        public Dictionary<ingredients, int> stock = new Dictionary<ingredients, int>();
        //public List<Employee> employees = new List<Employee>();
        public int employees;
        public List<recipes> recipes = new List<recipes>();
        
        public Shop()
        {

        }
        public Dictionary<string, int> StockPrint()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach(var i in stock)
            {
                dict.Add(i.Key.ingredient, i.Value);
            }
            return dict;
        }
    }
}
